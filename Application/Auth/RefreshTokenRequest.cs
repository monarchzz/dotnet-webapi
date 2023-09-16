using Application.Common.Auth;

namespace Application.Auth;

public record RefreshTokenRequest(string RefreshToken) : IRequest<TokenResponse>;

public class RefreshTokenRequestHandler : IRequestHandler<RefreshTokenRequest, TokenResponse>
{
    private readonly IReadRepository<User> _userRepository;
    private readonly IJwtTokenHelper _jwtTokenHelper;

    public RefreshTokenRequestHandler(IReadRepository<User> userRepository, IJwtTokenHelper jwtTokenHelper)
    {
        _userRepository = userRepository;
        _jwtTokenHelper = jwtTokenHelper;
    }

    public async Task<TokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var userId = _jwtTokenHelper.VerifyRefreshToken(request.RefreshToken);
        if (userId is null)
        {
            throw new UnauthorizedException("Token is invalid.");
        }

        var id = Guid.Parse(userId);
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedException("Token is invalid.");
        }

        return new TokenResponse(
            UserId: user.Id,
            Role: user.Role,
            Token: _jwtTokenHelper.GenerateToken(user),
            RefreshToken: _jwtTokenHelper.GenerateRefreshToken(user)
        );
    }
}