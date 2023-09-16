using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Auth;
using Domain.Catalog;
using Infrastructure.Auth.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Authorization;

namespace Infrastructure.Auth;

public class JwtTokenHelper : IJwtTokenHelper
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenHelper(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    private SigningCredentials GetSigningCredentials(string key)
    {
        byte[] secret = Encoding.UTF8.GetBytes(key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    private IEnumerable<Claim> GetClaims(User user) =>
        new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(AppClaims.Fullname, user.FullName),
            new(ClaimTypes.Name, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
        };

    private static string WriteToken(SecurityToken securityToken)
    {
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public string GenerateToken(User user)
    {
        var signingCredentials = GetSigningCredentials(_jwtSettings.Key);
        var claims = GetClaims(user);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            signingCredentials: signingCredentials
        );

        return WriteToken(token);
    }

    public string GenerateRefreshToken(User user)
    {
        var signingCredentials = GetSigningCredentials(_jwtSettings.RefreshKey);
        var claims = GetClaims(user);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpirationInDays),
            signingCredentials: signingCredentials
        );

        return WriteToken(securityToken);
    }

    public string? VerifyRefreshToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshKey))
        };
        try
        {
            var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out _);
            if (principal != null)
            {
                var id = principal.GetUserId();

                return id;
            }
        }
        catch (Exception)
        {
            return null;
        }

        return null;
    }
}