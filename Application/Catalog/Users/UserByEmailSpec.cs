namespace Application.Identity.Users;

public sealed class UserByEmailSpec : Specification<User>, ISingleResultSpecification<User>
{
    public UserByEmailSpec(string email)
    {
        Query.Where(u => u.Email == email);
    }
}