using Mapster;

namespace Application.Identity.Users;

public class UpdateProfileRequest : IRequest<UserDto>
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}

public class UpdateProfileRequestHandler : IRequestHandler<UpdateProfileRequest, UserDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<User> _userRepository;

    public UpdateProfileRequestHandler(ICurrentUser currentUser, IRepository<User> userRepository)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(_currentUser.GetUserId(), cancellationToken);
        if (user is null) throw new NotFoundException("User not found.");

        request.Adapt(user);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return user.Adapt<UserDto>();
    }
}