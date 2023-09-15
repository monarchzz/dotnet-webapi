using Microsoft.AspNetCore.Authorization;
using Shared.Authorization;

namespace Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string permission) =>
        Policy = Role.PermissionFor(permission);
}