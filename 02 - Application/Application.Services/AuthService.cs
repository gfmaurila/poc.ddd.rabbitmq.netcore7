using Application.Helper;
using Application.Request;
using Application.Responses;
using Ardalis.Result;
using AutoMapper;
using Domain.Contract.Repositories;
using Domain.Contract.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _repo;
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration,
                       IMapper mapper,
                       IUserRepository repo)
    {
        _mapper = mapper;
        _repo = repo;
        _configuration = configuration;
    }

    public async Task<Result<AuthResponses>> Auth(AuthRequest request)
    {
        var passwordHash = HashHelper.ComputeSha256Hash(request.Password);
        var user = await _repo.GetUserByEmailAndPasswordAsync(request.Email, passwordHash);

        if (user == null)
            return Result.NotFound($"Usuário ou senha inválido!");


        //Se existir, gera o token usando os dados do usuário
        var token = GenerateJwtToken(user.Email.Address, user.Role);

        return Result.Success(new AuthResponses(user.Email.Address, token));
    }

    public string GenerateJwtToken(string email, string role)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = _configuration["Jwt:Key"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
            {
                new Claim("userName", email),
                new Claim(ClaimTypes.Role, role)
            };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: credentials,
            claims: claims
            );

        var tokenHandler = new JwtSecurityTokenHandler();

        var stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }

}
