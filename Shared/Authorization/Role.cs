using System.Reflection;

namespace Shared.Authorization;

public static class Role
{
    public const string Admin = nameof(Admin);
    public const string Basic = nameof(Basic);


    public static List<string> GetAllRoles()
    {
        return typeof(Role).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi is { IsLiteral: true, IsInitOnly: false } && fi.FieldType == typeof(string))
            .Select(x => x.GetRawConstantValue())
            .Where(x => x is not null)
            .Cast<string>()
            .ToList();
    }

    public static bool IsValid(string role)
    {
        return GetAllRoles().Contains(role);
    }

    public static string PermissionFor(string permission) => $"{AppClaims.Permission}.{permission}";
}