using Application.Catalog.Users;
using Application.Common.Auth;
using Shared.Authorization;

namespace Application.Auth;

public class RegisterRequest : IRequest<TokenResponse>
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string Role { get; set; } = default!;
}

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(v => v.FirstName)
            .MaximumLength(200)
            .NotEmpty();
        RuleFor(v => v.LastName)
            .MaximumLength(200)
            .NotEmpty();
        RuleFor(v => v.Email)
            .MaximumLength(200)
            .NotEmpty()
            .EmailAddress();
        RuleFor(v => v.Password)
            .MaximumLength(200)
            .MinimumLength(6)
            .NotEmpty();
        RuleFor(v => v.Role)
            .MaximumLength(200)
            .NotEmpty();
    }
}

public class RegisterRequestHandler : IRequestHandler<RegisterRequest, TokenResponse>
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordHelper _passwordHelper;
    private readonly ISender _mediator;

    public RegisterRequestHandler(IRepository<User> userRepository, IPasswordHelper passwordHelper, ISender mediator)
    {
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
        _mediator = mediator;
    }

    public async Task<TokenResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var roleValid = Role.IsValid(request.Role);
        if (!roleValid) throw new BadRequestException("Role is not valid.");

        var userExists = await _userRepository.AnyAsync(new UserByEmailSpec(request.Email), cancellationToken);
        if (userExists) throw new BadRequestException("Email already exists.");

        var user = new User(request.FirstName, request.LastName, request.Email, request.Password, request.Role)
        {
            Password = _passwordHelper.HashPassword(request.Password)
        };
        await _userRepository.AddAsync(user, cancellationToken);

        return await _mediator.Send(
            new TokenRequest(
                Email: request.Email,
                Password: request.Password
            ),
            cancellationToken
        );
    }
}