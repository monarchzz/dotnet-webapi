using Mapster;

namespace Application.Catalog.Users;

public class UpdateUserRequest : IRequest<UserDto>
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}

public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, UserDto>
{
    private readonly IRepository<User> _userRepository;

    public UpdateUserRequestHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null) throw new NotFoundException("User not found.");

        request.Adapt(user);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return user.Adapt<UserDto>();
    }
}