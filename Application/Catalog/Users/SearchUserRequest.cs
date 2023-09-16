namespace Application.Catalog.Users;

public class SearchUsersRequest : PaginationFilter, IRequest<PaginationResponse<UserDto>>
{
}

public class SearchUsersRequestHandler : IRequestHandler<SearchUsersRequest, PaginationResponse<UserDto>>
{
    private readonly IReadRepository<User> _userRepository;

    public SearchUsersRequestHandler(IReadRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<PaginationResponse<UserDto>> Handle(SearchUsersRequest request, CancellationToken cancellationToken)
    {
        var spec = new UsersBySearchRequestSpec(request);

        return _userRepository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}