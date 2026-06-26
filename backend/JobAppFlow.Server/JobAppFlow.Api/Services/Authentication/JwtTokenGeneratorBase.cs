using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobAppFlow.Api.Models.Options;
using JobAppFlow.SqlDataAccess.Models;
using Microsoft.IdentityModel.Tokens;

namespace JobAppFlow.Api.Services.Authentication;

public abstract class JwtTokenGeneratorBase
{
    protected JwtOptions JwtOptions { get; }
    private readonly SigningCredentials _signingCredentials;

    protected JwtTokenGeneratorBase(JwtOptions jwtOptions)
    {
        ArgumentNullException.ThrowIfNull(jwtOptions);

        JwtOptions = jwtOptions;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Key));
        _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    protected string GenerateToken(
        IEnumerable<Claim> claims,
        DateTime expiresAtUtc)
    {
        var token = new JwtSecurityToken(
            issuer: JwtOptions.Issuer,
            audience: JwtOptions.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: _signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    protected static Claim[] CreateBaseClaims(ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
        ];
    }
}
