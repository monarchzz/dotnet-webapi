using Application.Identity.Users;
using Application.Identity.Users.Password;
using NSwag.Annotations;

namespace Host.Controllers.Personal;

public class PersonalController : VersionNeutralApiController
{
    [HttpGet("profile")]
    [OpenApiOperation("Get profile information.", "")]
    public Task<UserDto> GetProfileAsync(CancellationToken cancellationToken)
    {
        return Mediator.Send(new GetProfileRequest(), cancellationToken);
    }

    [HttpPut("profile")]
    [OpenApiOperation("Update profile information.", "")]
    public Task<UserDto> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        return Mediator.Send(request, cancellationToken);
    }

    [HttpPut("change-password")]
    [OpenApiOperation("Change password.", "")]
    public Task<UserDto> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        return Mediator.Send(request, cancellationToken);
    }
}