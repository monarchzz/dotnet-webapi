using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Auth.Jwt;

public class JwtSettings : IValidatableObject
{
    public string Key { get; set; } = string.Empty;

    public string RefreshKey { get; set; } = string.Empty;

    public int TokenExpirationInMinutes { get; set; }

    public int RefreshTokenExpirationInDays { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Key) || string.IsNullOrEmpty(RefreshKey))
        {
            yield return new ValidationResult("No Key defined in JwtSettings config", new[] { nameof(Key) });
        }
    }
}