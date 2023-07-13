using Application.Request;
using Application.Responses;
using Ardalis.Result;

namespace Domain.Contract.Services;
public interface IAuthService
{
    string GenerateJwtToken(string email, string role);
    Task<Result<AuthResponses>> Auth(AuthRequest request);
}
