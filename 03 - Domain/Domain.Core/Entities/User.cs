using Domain.Core.ValueObjects;

namespace Domain.Core.Entities;
public class User : BaseEntity
{
    public User()
    {
    }

    public User(string fullName, Email email, string phone, DateTime birthDate, string password, string role)
    {
        FullName = fullName;
        Email = email;
        Phone = phone;
        BirthDate = birthDate;
        Password = password;
        Role = role;
        Active = true;
    }

    public string FullName { get; private set; }
    public Email Email { get; private set; }
    public string Phone { get; private set; }
    public DateTime BirthDate { get; private set; }
    public bool Active { get; set; }
    public string Password { get; private set; }
    public string Role { get; private set; }
}
