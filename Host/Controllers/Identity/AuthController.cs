using Application.Auth;
using Microsoft.AspNetCore.Authorization;
using NSwag.Annotations;

namespace Host.Controllers.Identity;

public class AuthController : VersionNeutralApiController
{
    [HttpPost("login")]
    [AllowAnonymous]
    [OpenApiOperation("Request an access token using credentials.", "")]
    public Task<TokenResponse> GetTokenAsync(TokenRequest request, CancellationToken cancellationToken)
    {
        return Mediator.Send(request, cancellationToken);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [OpenApiOperation("Request an access token using a refresh token.", "")]
    public Task<TokenResponse> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        return Mediator.Send(request, cancellationToken);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [OpenApiOperation("Register a new user.", "")]
    public Task<TokenResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        return Mediator.Send(request, cancellationToken);
    }
}