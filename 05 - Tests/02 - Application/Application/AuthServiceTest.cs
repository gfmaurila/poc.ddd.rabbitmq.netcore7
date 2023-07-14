using Application.Helper;
using Application.Request;
using Application.Services;
using ApplicationTest.Fixture;
using AutoMapper;
using Domain.Contract.Producer;
using Domain.Contract.Repositories;
using Domain.Core.Entities;
using NSubstitute;


namespace ApplicationTest;

[Collection("AuthService collection")]
public class AuthServiceTest
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _repo;
    private readonly ICreateUserPerfilProducer _perfilProducer;
    private readonly AuthServiceFixture _fixture;
    private readonly AuthService _authService;
    public AuthRequest InvalidRequest { get; private set; }

    public AuthServiceTest(AuthServiceFixture fixture)
    {
        _mapper = Substitute.For<IMapper>();
        _repo = Substitute.For<IUserRepository>();
        _perfilProducer = Substitute.For<ICreateUserPerfilProducer>();
        _fixture = fixture;

        _authService = new AuthService(_fixture.Configuration, _mapper, _repo, _perfilProducer);
    }

    [Fact]
    public async Task Auth_ValidCredentials_ReturnsSuccessResult()
    {
        // Arrange
        _repo.GetUserByEmailAndPasswordAsync(_fixture.FakeUser.Email.Address, HashHelper.ComputeSha256Hash(_fixture.FakeUser.Password))
            .Returns(Task.FromResult(_fixture.FakeUser));

        var request = new AuthRequest { Email = _fixture.FakeUser.Email.Address, Password = _fixture.FakeUser.Password };

        // Act
        var result = await _authService.Auth(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(_fixture.FakeUser.Email.Address, result.Value.Email);
    }

    [Fact]
    public async Task Auth_InvalidCredentials_ReturnsNotFoundResult()
    {
        // Arrange
        _repo.GetUserByEmailAndPasswordAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(Task.FromResult((User)null));

        // Act
        var result = await _authService.Auth(_fixture.InvalidRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Value);
    }
}

