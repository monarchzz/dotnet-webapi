namespace Application.Common.Auth;

public interface IPasswordHelper : ITransientService
{
    string HashPassword(string password);

    bool VerifyHashedPassword(string hashedPassword, string password);
}