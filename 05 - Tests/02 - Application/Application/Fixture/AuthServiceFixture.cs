using Application.Request;
using Domain.Core.Entities;
using Domain.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace ApplicationTest.Fixture;
public class AuthServiceFixture
{
    public IConfiguration Configuration { get; private set; }
    public User FakeUser { get; private set; }
    public AuthRequest InvalidRequest { get; private set; }

    public AuthServiceFixture()
    {
        Configuration = Substitute.For<IConfiguration>();
        Configuration["Jwt:Key"].Returns("thisIsASecretKey");

        var email = new Email("test@email.com");
        FakeUser = new User("Test User", email, "1234567890", DateTime.Now, "password", "Admin");

        InvalidRequest = new AuthRequest { Email = "invalid@email.com", Password = "invalid" };
    }
}