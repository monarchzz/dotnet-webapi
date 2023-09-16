namespace Application.Identity.Tokens;

public record TokenResponse(
    Guid UserId,
    string Role,
    string Token,
    string RefreshToken
);