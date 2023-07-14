using Application.DTOs;
using Application.Helper;
using Application.Responses;
using Application.Validators;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using AutoMapper;
using Domain.Contract.Producer;
using Domain.Contract.Redis;
using Domain.Contract.Repositories;
using Domain.Contract.Services;
using Domain.Core.Entities;
using Domain.Core.Model;
using Domain.Core.ValueObjects;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;
public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly ICreateUserAllProducer _producesAll;
    private readonly ICreateUserProducer _producesCreateUser;
    private readonly IDeleteUserProducer _producesDeleteUser;
    private readonly IUserRepository _repo;
    private readonly ICacheRepository _repoCache;
    private readonly CreateUserValidator _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(ICreateUserAllProducer producesAll,
                       ICreateUserProducer producesCreateUser,
                       IDeleteUserProducer producesDeleteUser,
                       CreateUserValidator validator,
                       IMapper mapper,
                       IUserRepository repo,
                       IUnitOfWork unitOfWork,
                       ICacheRepository repoCache)
    {
        _mapper = mapper;
        _producesAll = producesAll;
        _producesCreateUser = producesCreateUser;
        _producesDeleteUser = producesDeleteUser;
        _repo = repo;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _repoCache = repoCache;

    }

    #region Get and GetById
    public async Task<Result<List<UserListDto>>> Get()
    {
        var userRedis = await _repoCache.StringGetAllAsync<UserListDto>(0);

        if (!userRedis.IsNullOrEmpty())
            return Result.Success(userRedis);

        _producesAll.Publish();
        return Result.Success(_mapper.Map<List<UserListDto>>(await _repo.Get()));
    }

    public async Task<Result<PaginationResult<UserListDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var userRedis = await _repoCache.StringGetAllAsync<UserListDto>(pageNumber, pageSize, 0);

        if (!userRedis.Data.IsNullOrEmpty())
            return Result.Success(userRedis);

        var result = await _repo.GetAllAsync(pageNumber, pageSize);

        var userListDto = _mapper.Map<List<UserListDto>>(result.Data);

        _producesAll.Publish();

        return new PaginationResult<UserListDto>
        {
            Data = userListDto,
            TotalCount = result.TotalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            QtdPge = result.QtdPge
        };
    }


    public async Task<Result<UserListDto>> Get(int id)
    {
        var userRedis = await _repoCache.StringGetAsync<UserListDto>($"user_id_{id}", 0);

        if (userRedis != null)
            return Result.Success(userRedis);

        var objEntity = _mapper?.Map<UserListDto>(await _repo.Get(id));
        if (objEntity == null)
            return Result.NotFound($"Nenhum registro encontrado pelo Id: {id}");

        _producesCreateUser.Publish(objEntity);

        return Result.Success(objEntity);
    }
    #endregion

    #region Create, Update and Delete
    public async Task<Result<CreatedUserResponse>> Create(UserDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        if (await _repo.ExistsByEmailAsync(new Email(dto.Email)))
            return Result.Error("O endereço de e-mail informado já está sendo utilizado.");

        dto.Password = HashHelper.ComputeSha256Hash(dto.Password);
        var entityCreated = await _repo.Create(_mapper.Map<User>(dto));
        await _unitOfWork.SaveChangesAsync();

        _producesCreateUser.Publish(_mapper.Map<UserListDto>(entityCreated));

        return Result.Success(new CreatedUserResponse(entityCreated.Id), "Cadastrado com sucesso!");
    }

    public async Task<Result> Update(UserDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.AsErrors());

        var objEntity = await _repo.Get(dto.Id);
        if (objEntity == null)
            return Result.NotFound($"Nenhum registro encontrado pelo Id: {dto.Id}");

        if (await _repo.ExistsByEmailAsync(new Email(dto.Email), objEntity.Id))
            return Result.Error("O endereço de e-mail informado já está sendo utilizado.");

        dto.Password = HashHelper.ComputeSha256Hash(dto.Password);
        var entityUpdate = await _repo.Update(_mapper.Map<User>(dto));
        await _unitOfWork.SaveChangesAsync();

        _producesCreateUser.Publish(_mapper.Map<UserListDto>(entityUpdate));

        return Result.SuccessWithMessage("Atualizado com sucesso!");
    }

    public async Task<Result> Remove(int id)
    {
        // Obtendo o registro da base.
        var objEntity = await _repo.Get(id);
        if (objEntity == null)
            return Result.NotFound($"Nenhum registro encontrado pelo Id: {id}");

        _producesDeleteUser.Publish(_mapper.Map<UserListDto>(objEntity));

        await _repo.Remove(id);
        await _unitOfWork.SaveChangesAsync();

        _producesAll.Publish();
        return Result.SuccessWithMessage("Removido com sucesso!");
    }
    #endregion

    #region CreateRedis
    public async Task CreateRedis()
    {
        var users = await _repo.Get();
        foreach (var user in users)
        {
            var mapperdto = _mapper.Map<UserDto>(user);
            await _repoCache.CreateBatch($"user_id_{mapperdto.Id}", mapperdto, TimeSpan.FromHours(1), 0);
        }
    }

    public async Task CreateRedis(string key, UserListDto dto, int db)
    {
        await _repoCache.SetAsync(key, dto, db);
    }
    #endregion
}
