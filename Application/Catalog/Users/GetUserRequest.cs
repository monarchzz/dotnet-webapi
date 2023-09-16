using Mapster;

namespace Application.Catalog.Users;

public class GetUserRequest : IRequest<UserDto>
{
    public Guid Id { get; set; }
}

public class GetUserRequestHandler : IRequestHandler<GetUserRequest, UserDto>
{
    private readonly IReadRepository<User> _userRepository;

    public GetUserRequestHandler(IReadRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        return user is null
            ? throw new NotFoundException("User not found.")
            : user.Adapt<UserDto>();
    }
}