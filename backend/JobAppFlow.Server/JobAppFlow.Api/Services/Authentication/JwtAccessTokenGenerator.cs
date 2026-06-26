using System.IdentityModel.Tokens.Jwt;
using JobAppFlow.Api.Models.Options;
using JobAppFlow.SqlDataAccess.Models;

namespace JobAppFlow.Api.Services.Authentication;

public sealed class JwtAccessTokenGenerator : JwtTokenGeneratorBase, IAccessTokenGenerator
{
    public JwtAccessTokenGenerator(JwtOptions jwtOptions)
        : base(jwtOptions) { }

    public string Generate(ApplicationUser user)
    {
        var claims = CreateBaseClaims(user);
        return GenerateToken(claims, DateTime.UtcNow.AddMinutes(JwtOptions.AccessTokenExpirationMinutes));
    }
}
