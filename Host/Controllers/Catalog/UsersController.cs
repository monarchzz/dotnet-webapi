using Application.Common.Exceptions;
using Application.Identity.Users;
using NSwag.Annotations;

namespace Host.Controllers.Catalog;

public class UsersController : VersionedApiController
{
    [HttpPost]
    [OpenApiOperation("Create a new user.", "")]
    public Task<BaseDto> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        return Mediator.Send(request, cancellationToken);
    }

    [HttpPut("{id:guid}")]
    [OpenApiOperation("Update an existing user.", "")]
    public Task<UserDto> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        return id != request.Id
            ? throw new BadRequestException("The id in the url does not match the id in the request.")
            : Mediator.Send(request, cancellationToken);
    }

    [HttpDelete("{id:guid}")]
    [OpenApiOperation("Delete an existing user.", "")]
    public Task<BaseDto> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return Mediator.Send(new DeleteUserRequest(id), cancellationToken);
    }
}