using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JobAppFlow.Api.Constants;
using JobAppFlow.Api.Models.Options;
using JobAppFlow.SqlDataAccess.Models;

namespace JobAppFlow.Api.Services.Authentication;

public sealed class JwtRefreshTokenGenerator : JwtTokenGeneratorBase, IRefreshTokenJwtGenerator
{
    public JwtRefreshTokenGenerator(JwtOptions jwtOptions, JwtKeys jwtKeys)
        : base(jwtOptions, jwtKeys.RefreshKey)
    {
    }

    public string Generate(ApplicationUser user)
    {
        var claims = CreateBaseClaims(user, JwtTokenTypes.Refresh);
        return GenerateToken(claims, DateTime.UtcNow.AddDays(JwtOptions.RefreshTokenExpirationDays));
    }
}
