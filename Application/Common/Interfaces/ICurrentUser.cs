using System.Security.Claims;

namespace Application.Common.Interfaces;

public interface ICurrentUser
{
    string? Name { get; }

    Guid GetUserId();

    string? GetUserEmail();

    bool IsAuthenticated();

    IEnumerable<Claim>? GetUserClaims();
}