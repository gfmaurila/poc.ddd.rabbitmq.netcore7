using Newtonsoft.Json;

namespace Application.DTOs;

public class UserListDto
{
    public UserListDto()
    {
    }

    public UserListDto(int id)
    {
        Id = id;
    }

    [JsonConstructor]
    public UserListDto(int id, string fullName, string email, string phone, DateTime birthDate, DateTime modified, bool active, string role)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        Phone = phone;
        BirthDate = birthDate;
        Modified = modified;
        Active = active;
        Role = role;
    }

    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime Modified { get; set; }
    public bool Active { get; set; }
    public string Role { get; set; }
}
