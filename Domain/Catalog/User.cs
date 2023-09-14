using Shared.Authorization;

namespace Domain.Catalog;

public class User : IAggregateRoot
{
    public User(string firstName, string lastName, string email, string password, Role role)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Role = role;
    }

    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
    
    public Role Role { get; set; }
}