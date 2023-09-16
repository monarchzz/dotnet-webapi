namespace Application.Identity.Users;

public class DeleteUserRequest : IRequest<BaseDto>
{
    public DeleteUserRequest(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class DeleteUserRequestHandler : IRequestHandler<DeleteUserRequest, BaseDto>
{
    private readonly IRepository<User> _userRepository;

    public DeleteUserRequestHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<BaseDto> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        await _userRepository.DeleteAsync(user, cancellationToken);
        return new BaseDto(user.Id);
    }
}