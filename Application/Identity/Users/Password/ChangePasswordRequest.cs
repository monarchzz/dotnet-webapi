using Application.Common.Auth;
using Mapster;

namespace Application.Identity.Users.Password;

public class ChangePasswordRequest : IRequest<UserDto>
{
    public string Password { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}

public class ChangePasswordRequestValidator : CustomValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(p => p.Password)
            .MinimumLength(6);

        RuleFor(p => p.NewPassword)
            .MinimumLength(6);
    }
}

public class ChangePasswordHandler : IRequestHandler<ChangePasswordRequest, UserDto>
{
    private readonly IPasswordHelper _passwordHelper;
    private readonly IRepository<User> _userRepository;
    private readonly ICurrentUser _currentUser;

    public ChangePasswordHandler(
        IPasswordHelper passwordHelper,
        IRepository<User> userRepository,
        ICurrentUser currentUser
    )
    {
        _passwordHelper = passwordHelper;
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<UserDto> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(_currentUser.GetUserId(), cancellationToken);
        if (user is null) throw new NotFoundException("User not found.");

        if (!_passwordHelper.VerifyHashedPassword(user.Password, request.Password))
            throw new BadRequestException("Invalid password.");

        user.Password = _passwordHelper.HashPassword(request.NewPassword);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return user.Adapt<UserDto>();
    }
}