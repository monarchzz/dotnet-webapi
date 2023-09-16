using Application.Catalog.Users;
using Application.Common.Persistence;
using Domain.Catalog;
using Microsoft.AspNetCore.Authorization;
using Shared.Authorization;

namespace Infrastructure.Auth.Permissions;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IReadRepository<User> _repository;

    public PermissionAuthorizationHandler(IReadRepository<User> repository)
    {
        _repository = repository;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement
    )
    {
        var requirementArray = requirement.Permission.Split(".");
        if (requirementArray.Length < 2)
            return;

        var permissions = requirementArray[1].Split(",");

        var userId = context.User?.GetUserId();
        if (userId is null)
            return;
        var user = await _repository.GetByIdAsync(Guid.Parse(userId));
        if (user is null)
            return;
        if (permissions.Any(p => string.Equals(p, user.Role)))
        {
            context.Succeed(requirement);
        }
    }
}