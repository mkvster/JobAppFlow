using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JobAppFlow.Api.Models.Options;
using JobAppFlow.SqlDataAccess.Models;

namespace JobAppFlow.Api.Services.Authentication;

public sealed class JwtRefreshTokenGenerator : JwtTokenGeneratorBase, IRefreshTokenJwtGenerator
{
    public JwtRefreshTokenGenerator(JwtOptions jwtOptions)
        : base(jwtOptions) { }

    public string Generate(ApplicationUser user)
    {
        var claims = CreateBaseClaims(user)
            .Append(new Claim(JwtRegisteredClaimNames.Typ, "refresh"))
            .ToArray();

        return GenerateToken(claims, DateTime.UtcNow.AddDays(JwtOptions.RefreshTokenExpirationDays));
    }
}
