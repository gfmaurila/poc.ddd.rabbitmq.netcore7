namespace Application.Responses;

public class AuthResponses
{
    public AuthResponses(string email, string token)
    {
        Email = email;
        Token = token;
    }
    public string Email { get; set; }
    public string Token { get; set; }
}