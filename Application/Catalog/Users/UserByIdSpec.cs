namespace Application.Catalog.Users;

public sealed class UserByIdSpec : Specification<User>, ISingleResultSpecification<User>
{
    public UserByIdSpec(Guid userId) =>
        Query.Where(u => u.Id == userId);

    public UserByIdSpec(string userId) =>
        Query.Where(u => u.Id == Guid.Parse(userId));
}