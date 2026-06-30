using System.Security.Claims;
using JobAppFlow.Api.Constants;
using JobAppFlow.Api.Models.Options;
using JobAppFlow.SqlDataAccess.Models;

namespace JobAppFlow.Api.Services.Authentication;

public sealed class JwtAccessTokenGenerator : JwtTokenGeneratorBase, IAccessTokenGenerator
{
    public JwtAccessTokenGenerator(JwtOptions jwtOptions, JwtKeys jwtKeys)
        : base(jwtOptions, jwtKeys.AccessKey)
    {
    }

    public string Generate(ApplicationUser user, IEnumerable<string>? roles = null)
    {
        var claims = CreateBaseClaims(user, JwtTokenTypes.Access).ToList();

        if (roles is not null)
        {
            claims.AddRange(roles
                .Where(role => !string.IsNullOrWhiteSpace(role))
                .Select(role => new Claim(ClaimTypes.Role, role.Trim())));
        }

        return GenerateToken(claims, DateTime.UtcNow.AddMinutes(JwtOptions.AccessTokenExpirationMinutes));
    }
}
