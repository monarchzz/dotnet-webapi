using Application.Common.Auth;
using Shared.Authorization;

namespace Application.Catalog.Users;

public class CreateUserRequest : IRequest<BaseDto>
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string Role { get; set; } = default!;
}

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
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

public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, BaseDto>
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordHelper _passwordHelper;

    public CreateUserRequestHandler(IRepository<User> userRepository, IPasswordHelper passwordHelper)
    {
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
    }

    public async Task<BaseDto> Handle(CreateUserRequest request, CancellationToken cancellationToken)
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

        return new BaseDto(user.Id);
    }
}