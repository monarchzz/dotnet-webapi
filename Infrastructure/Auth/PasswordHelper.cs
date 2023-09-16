using Application.Common.Auth;
using BC = BCrypt.Net.BCrypt;

namespace Infrastructure.Auth;

public class PasswordHelper : IPasswordHelper
{
    public PasswordHelper()
    {
    }

    public string HashPassword(string password)
    {
        return BC.HashPassword(password);
    }

    public bool VerifyHashedPassword(string hashedPassword, string password)
    {
        return BC.Verify(password, hashedPassword);
    }
}