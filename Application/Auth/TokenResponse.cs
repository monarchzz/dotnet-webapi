namespace Application.Auth;

public record TokenResponse(
    Guid UserId,
    string Role,
    string Token,
    string RefreshToken
);