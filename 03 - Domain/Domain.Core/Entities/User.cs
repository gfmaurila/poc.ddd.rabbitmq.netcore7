using Domain.Core.ValueObjects;

namespace Domain.Core.Entities;
public class User : BaseEntity
{
    public string FullName { get; private set; }
    public Email Email { get; private set; }
    public string Phone { get; private set; }
    public DateTime BirthDate { get; private set; }
    public bool Active { get; set; }
    public string Password { get; private set; }
    public string Role { get; private set; }
}
