using Application.Request;
using Application.Responses;
using Ardalis.Result;
using Domain.Contract.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WebAPI.Controllers;
using WebAPI.Models;

namespace ApplicationTest;


public class AuthControllerTests
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _service;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _logger = Substitute.For<ILogger<AuthController>>();
        _service = Substitute.For<IAuthService>();
        _controller = new AuthController(_logger, _service);
    }

    [Fact]
    public async Task Login_ShouldReturnOkResponseWithAuthResponses()
    {
        // Arrange
        var request = new AuthRequest
        {
            // Preencha os campos do objeto AuthRequest conforme necessário
        };

        var expectedResponse = new AuthResponses("user@example.com", "token");

        _service.Auth(request).Returns(Task.FromResult(Result<AuthResponses>.Success(expectedResponse)));

        // Act
        var result = await _controller.Login(request) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        var response = result.Value as ApiResponse<AuthResponses>;
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.Equal(expectedResponse.Email, response.Result.Email);
        Assert.Equal(expectedResponse.Token, response.Result.Token);
    }
}
