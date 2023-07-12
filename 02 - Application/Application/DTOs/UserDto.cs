namespace Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime Modified { get; set; }
    public bool Active { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
