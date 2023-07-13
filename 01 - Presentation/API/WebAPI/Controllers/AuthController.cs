using Application.Request;
using Application.Responses;
using Domain.Contract.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebAPI.Configuration;
using WebAPI.Models;

namespace WebAPI.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _service;
    public AuthController(ILogger<AuthController> logger,
                          IAuthService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// Cadastra um novo usuário.
    /// </summary>
    /// <param name="request"></param>
    /// <response code="200">Retorna o email e token do usuário.</response>
    /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
    /// <response code="500">Quando ocorre um erro interno inesperado no servidor.</response>
    [HttpPost("login")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponses>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
        => (await _service.Auth(request)).ToActionResult();

}
