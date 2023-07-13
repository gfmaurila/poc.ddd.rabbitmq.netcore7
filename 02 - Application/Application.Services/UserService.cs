using Application.DTOs;
using Application.Helper;
using Application.Responses;
using Application.Validators;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using AutoMapper;
using Domain.Contract.Redis;
using Domain.Contract.Repositories;
using Domain.Contract.Services;
using Domain.Core.Entities;
using Domain.Core.ValueObjects;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;
public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _repo;
    private readonly ICacheRepository _repoCache;
    private readonly CreateUserValidator _validator;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(CreateUserValidator validator,
                       IMapper mapper,
                       IUserRepository repo,
                       IUnitOfWork unitOfWork,
                       ICacheRepository repoCache)
    {
        _mapper = mapper;
        _repo = repo;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _repoCache = repoCache;
    }

    public async Task<Result<UserDto>> Get(int id)
    {
        var userRedis = await _repoCache.StringGetAsync<UserDto>($"user_id_{id}");

        if (userRedis != null)
            return Result.Success(userRedis);

        var objEntity = _mapper.Map<UserDto>(await _repo.Get(id));
        if (objEntity == null)
            return Result.NotFound($"Nenhum registro encontrado pelo Id: {id}");

        // Add Fila para armazenar no redis 
        var mapperdto = _mapper.Map<UserDto>(objEntity);
        await _repoCache.SetAsync($"user_id_{mapperdto.Id}", mapperdto);

        return Result.Success(objEntity);
    }

    public async Task<Result<List<UserDto>>> Get()
    {
        var userRedis = await _repoCache.StringGetAllAsync<UserDto>();

        if (!userRedis.IsNullOrEmpty())
            return Result.Success(userRedis);

        // implementar um envio 
        return Result.Success(_mapper.Map<List<UserDto>>(await _repo.Get()));
    }

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

        // Add Fila para armazenar no redis 
        var mapperdto = _mapper.Map<UserDto>(entityCreated);
        await _repoCache.SetAsync($"user_id_{mapperdto.Id}", mapperdto);

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

        // Add Fila para armazenar no redis 
        var mapperdto = _mapper.Map<UserDto>(entityUpdate);
        await _repoCache.SetAsync($"user_id_{mapperdto.Id}", mapperdto);

        return Result.SuccessWithMessage("Atualizado com sucesso!");
    }

    public async Task<Result> Remove(int id)
    {
        // Obtendo o registro da base.
        var objEntity = await _repo.Get(id);
        if (objEntity == null)
            return Result.NotFound($"Nenhum registro encontrado pelo Id: {id}");

        // Add BKP por mais 30 dias esse redistro removido
        // Add Fila para armazenar no redis 
        var mapperdto = _mapper.Map<UserDto>(objEntity);
        await _repoCache.SetAsync($"user_delete_id_{mapperdto.Id}", mapperdto);

        await _repo.Remove(id);
        await _unitOfWork.SaveChangesAsync();

        // Add Fila para armazenar no redis 
        return Result.SuccessWithMessage("Removido com sucesso!");
    }
}
