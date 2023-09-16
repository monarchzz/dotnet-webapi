using Application.Catalog.Users;
using Mapster;

namespace Application.Identity.Users;

public class GetProfileRequest : IRequest<UserDto>
{
}

public class GetProfileRequestHandler : IRequestHandler<GetProfileRequest, UserDto>
{
    private readonly IReadRepository<User> _userRepository;
    private readonly ICurrentUser _currentUser;

    public GetProfileRequestHandler(IReadRepository<User> userRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<UserDto> Handle(GetProfileRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(
            new UserByIdSpec(_currentUser.GetUserId()),
            cancellationToken
        );
        return user is null ? throw new NotFoundException("User not found.") : user.Adapt<UserDto>();
    }
}