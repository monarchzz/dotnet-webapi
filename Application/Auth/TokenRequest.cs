using Application.Catalog.Users;
using Application.Common.Auth;

namespace Application.Auth;

public record TokenRequest(string Email, string Password) : IRequest<TokenResponse>;

public class TokenRequestValidator : CustomValidator<TokenRequest>
{
    public TokenRequestValidator()
    {
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid Email Address.");

        RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}

public class TokenRequestHandler : IRequestHandler<TokenRequest, TokenResponse>
{
    private readonly IReadRepository<User> _userRepository;
    private readonly IJwtTokenHelper _jwtTokenHelper;
    private readonly IPasswordHelper _passwordHelper;

    public TokenRequestHandler(
        IReadRepository<User> userRepository,
        IJwtTokenHelper jwtTokenHelper,
        IPasswordHelper passwordHelper
    )
    {
        _userRepository = userRepository;
        _jwtTokenHelper = jwtTokenHelper;
        _passwordHelper = passwordHelper;
    }

    public async Task<TokenResponse> Handle(TokenRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(new UserByEmailSpec(request.Email), cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedException("Authentication Failed.");
        }

        if (!_passwordHelper.VerifyHashedPassword(user.Password, request.Password))
        {
            throw new UnauthorizedException("Authentication Failed.");
        }

        return new TokenResponse(
            UserId: user.Id,
            Role: user.Role,
            Token: _jwtTokenHelper.GenerateToken(user),
            RefreshToken: _jwtTokenHelper.GenerateRefreshToken(user)
        );
    }
}