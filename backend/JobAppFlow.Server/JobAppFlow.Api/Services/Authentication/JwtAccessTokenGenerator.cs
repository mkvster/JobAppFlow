using System.IdentityModel.Tokens.Jwt;
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

    public string Generate(ApplicationUser user)
    {
        var claims = CreateBaseClaims(user, JwtTokenTypes.Access);
        return GenerateToken(claims, DateTime.UtcNow.AddMinutes(JwtOptions.AccessTokenExpirationMinutes));
    }
}
