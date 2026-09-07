using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.IdentityModel.Tokens;

namespace Legislature.TrackingSystem.Web.Auth;

/// <summary>
/// Issues signed JWT bearer tokens for registered DOR users (US-9.1.1, B.COM.06). The token
/// carries the user's role claim so HTTP-layer authorization policies can enforce the
/// role-to-permission matrix without re-querying the user store on every request.
/// </summary>
public sealed class JwtTokenService
{
    private readonly IAuthorizationService _authorization;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _key;

    public JwtTokenService(IAuthorizationService authorization, IConfiguration configuration)
    {
        _authorization = authorization;
        _issuer = configuration["Jwt:Issuer"] ?? "Legislature.TrackingSystem";
        _audience = configuration["Jwt:Audience"] ?? "Legislature.TrackingSystem.Web";
        string key = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key is not configured.");
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    }

    public async Task<JwtTokenResult> IssueTokenAsync(string userKey, CancellationToken cancellationToken)
    {
        IReadOnlyList<UserAccountDto> users = await _authorization.ListUsersAsync(cancellationToken);
        UserAccountDto? user = users.FirstOrDefault(u => string.Equals(u.UserKey, userKey, StringComparison.OrdinalIgnoreCase));
        if (user is null || !user.IsActive)
        {
            throw new InvalidOperationException($"User '{userKey}' is not registered or is inactive.");
        }

        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset expires = now.AddHours(8);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserKey),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserKey),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expires.UtcDateTime,
            signingCredentials: credentials);

        string serialized = new JwtSecurityTokenHandler().WriteToken(token);
        return new JwtTokenResult(serialized, user.UserKey, user.Role, expires);
    }
}

/// <summary>A successfully issued JWT bearer token.</summary>
public sealed record JwtTokenResult(string Token, string UserKey, UserRole Role, DateTimeOffset ExpiresAt);
