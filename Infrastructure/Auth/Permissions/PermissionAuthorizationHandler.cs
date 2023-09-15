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

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User?.GetUserId() is { } userId &&
            await _repository.AnyAsync(new UserByIdSpec(userId))
           )
        {
            context.Succeed(requirement);
        }
    }
}