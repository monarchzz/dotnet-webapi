namespace Shared.Authorization;

public static class Role
{
    public const string Admin = nameof(Admin);
    public const string Basic = nameof(Basic);


    public static string PermissionFor(string permission) => $"${AppClaims.Permission}.{permission}";
}