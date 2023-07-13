using Application.DTOs;
using Application.Services;
using Application.Validators;
using ApplicationTest.Fixture;
using Ardalis.Result;
using AutoMapper;
using Domain.Contract.Producer;
using Domain.Contract.Redis;
using Domain.Contract.Repositories;
using Domain.Core.Entities;
using NSubstitute;

namespace ApplicationTest;

public class UserServiceTests
{
    private IMapper _mapper;
    private ICreateUserAllProducer _producesAll;
    private ICreateUserProducer _producesCreateUser;
    private IDeleteUserProducer _producesDeleteUser;
    private IUserRepository _repo;
    private ICacheRepository _repoCache;
    private CreateUserValidator _validator;
    private IUnitOfWork _unitOfWork;

    private UserService _userService;

    public UserServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfileTest>();
        });

        _mapper = config.CreateMapper();

        _producesAll = Substitute.For<ICreateUserAllProducer>();
        _producesCreateUser = Substitute.For<ICreateUserProducer>();
        _producesDeleteUser = Substitute.For<IDeleteUserProducer>();
        _repo = Substitute.For<IUserRepository>();
        _repoCache = Substitute.For<ICacheRepository>();
        _validator = Substitute.For<CreateUserValidator>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _userService = new UserService(
            _producesAll,
            _producesCreateUser,
            _producesDeleteUser,
            _validator,
            _mapper,
            _repo,
            _unitOfWork,
            _repoCache
        );
    }


    [Fact]
    public void TestMethod_ShouldDoSomething_WhenSomethingHappens()
    {
        // Arrange
        // Configurar suas substituições e variáveis de entrada aqui.
        // Por exemplo: _repo.GetById(Arg.Any<int>()).Returns(new User());

        // Act
        // Chamar o método que você está testando.
        // Por exemplo: var result = _userService.TestMethod(input);

        // Assert
        // Verificar o resultado do teste.
        // Por exemplo: Assert.Equal(expected, result);
    }

    #region GET
    [Fact]
    public async Task Get_ShouldReturnUsersFromCache_WhenCacheIsNotEmpty()
    {
        // Arrange
        var expectedUsers = new List<UserListDto> {
            new UserListDto {
                Id = 1,
                FullName = "User1",
                Email = "user1@example.com",
                Phone = "123-456-789001",
                BirthDate = DateTime.Parse("2003-07-13T00:00:00"),
                Modified = DateTime.Parse("2023-07-13T17:13:56.83"),
                Active = true,
                Role = "Admin"
            }
        };
        _repoCache.StringGetAllAsync<UserListDto>(0).Returns(expectedUsers);

        // Act
        var result = await _userService.Get();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedUsers, result.Value);
        _repo.DidNotReceive().Get();
        _producesAll.DidNotReceive().Publish();
    }


    #endregion

    #region GET BY ID
    [Fact]
    public async Task Get_ShouldReturnUserFromCache_WhenUserExistsInCache()
    {
        // Arrange
        var expectedUser = new UserListDto
        {
            Id = 1,
            FullName = "User1",
            Email = "user1@example.com",
            Phone = "123-456-789001",
            BirthDate = DateTime.Parse("2003-07-13T00:00:00"),
            Modified = DateTime.Parse("2023-07-13T17:13:56.83"),
            Active = true,
            Role = "Admin"
        };
        _repoCache.StringGetAsync<UserListDto>(Arg.Any<string>(), 0).Returns(expectedUser);

        // Act
        var result = await _userService.Get(expectedUser.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.Equal(expectedUser, result.Value);
        _repo.DidNotReceive().Get(expectedUser.Id);
        _producesCreateUser.DidNotReceive().Publish(Arg.Any<UserListDto>());
    }


    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        int nonExistentId = 200;
        _repoCache.StringGetAsync<UserListDto>(Arg.Any<string>(), 0).Returns((UserListDto)null);
        _repo.Get(nonExistentId).Returns((User)null);

        // Act
        var result = await _userService.Get(nonExistentId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Contains($"Nenhum registro encontrado pelo Id: {nonExistentId}", result.Errors);
        _producesCreateUser.DidNotReceive().Publish(Arg.Any<UserListDto>());
    }
    #endregion

}


