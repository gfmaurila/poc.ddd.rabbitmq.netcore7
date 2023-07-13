using Application.DTOs;
using Domain.Core.Entities;
using Domain.Core.ValueObjects;

namespace ApplicationTest.Fixture;


public class UserServiceFixture
{
    public User FakeUser { get; }
    public UserDto FakeCreateUserDto { get; }
    public UserListDto FakeUserDto { get; }

    public UserServiceFixture()
    {
        var email = new Email("test@email.com");
        FakeUser = new User("Test User", email, "1234567890", DateTime.Now, "password", "Admin");
        FakeUserDto = new UserListDto { Id = 1, FullName = "Test User" };

        FakeCreateUserDto = new UserDto
        {
            Id = 1,
            Email = "test@email.com",
            Password = "password",
            Role = "Admin",
            // Adicione as outras propriedades necessárias
        };
    }
}
