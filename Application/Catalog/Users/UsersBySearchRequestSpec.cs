namespace Application.Catalog.Users;

public sealed class UsersBySearchRequestSpec : EntitiesByPaginationFilterSpec<User, UserDto>
{
    public UsersBySearchRequestSpec(PaginationFilter request)
        : base(request)
    {
        Query.OrderBy(q => q.FullName, !request.HasOrderBy());
    }
}