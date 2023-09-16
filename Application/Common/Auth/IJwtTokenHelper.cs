namespace Application.Common.Auth;

public interface IJwtTokenHelper : ITransientService
{
    string GenerateToken(User user);

    string GenerateRefreshToken(User user);

    string? VerifyRefreshToken(string refreshToken);
}