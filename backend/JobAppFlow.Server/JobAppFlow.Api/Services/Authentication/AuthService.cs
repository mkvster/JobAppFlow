using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobAppFlow.Api.Constants;
using JobAppFlow.Api.Models.Auth;
using JobAppFlow.Api.Models.Options;
using JobAppFlow.SqlDataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace JobAppFlow.Api.Services.Authentication;

public sealed class AuthService : IAuthService
{
    private static readonly JwtSecurityTokenHandler TokenHandler = new();

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenJwtGenerator _refreshTokenGenerator;
    private readonly AuthOptions _authOptions;
    private readonly DemoOptions _demoOptions;
    private readonly JwtOptions _jwtOptions;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenJwtGenerator refreshTokenGenerator,
        AuthOptions authOptions,
        DemoOptions demoOptions,
        JwtOptions jwtOptions,
        JwtKeys jwtKeys)
    {
        _userManager = userManager;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _authOptions = authOptions;
        _demoOptions = demoOptions;
        _jwtOptions = jwtOptions;
        _tokenValidationParameters = CreateTokenValidationParameters(jwtOptions, jwtKeys);
    }

    public async Task<AuthSessionResult?> LoginAsync(
        string emailOrUsername,
        string password,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await FindUserAsync(emailOrUsername, cancellationToken);
        if (user is null)
        {
            return null;
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
        {
            return null;
        }

        return await IssueSessionAsync(user, cancellationToken);
    }

    public async Task<AuthSessionResult?> LoginDemoAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var demoLogin = _demoOptions.DemoUserLogin.Trim();
        if (string.IsNullOrWhiteSpace(demoLogin))
        {
            return null;
        }

        var user = await FindUserAsync(demoLogin, cancellationToken);
        if (user is null)
        {
            return null;
        }

        var isDemo = await _userManager.IsInRoleAsync(user, ApplicationRoleNames.Demo);
        if (!isDemo)
        {
            return null;
        }

        return await IssueSessionAsync(user, cancellationToken);
    }

    public async Task<AuthSessionResult?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return null;
        }

        if (!TryValidateRefreshToken(refreshToken, out var principal))
        {
            return null;
        }

        var userIdValue = GetSubjectId(principal);
        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return null;
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return null;
        }

        return await IssueSessionAsync(user, cancellationToken);
    }

    public Task LogoutAsync(string? refreshToken, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _ = refreshToken;
        return Task.CompletedTask;
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync(
        ClaimsPrincipal principal,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userIdValue = GetSubjectId(principal);
        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return null;
        }

        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<string[]> GetCurrentUserRolesAsync(
        ClaimsPrincipal principal,
        CancellationToken cancellationToken = default)
    {
        var user = await GetCurrentUserAsync(principal, cancellationToken);
        if (user is null)
        {
            return Array.Empty<string>();
        }

        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToArray();
    }

    private async Task<AuthSessionResult> IssueSessionAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var roles = await _userManager.GetRolesAsync(user);
        var roleNames = roles.ToArray();
        var refreshToken = _refreshTokenGenerator.Generate(user);
        var refreshTokenExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);

        return new AuthSessionResult(
            _accessTokenGenerator.Generate(user, roleNames),
            refreshToken,
            refreshTokenExpiresAtUtc,
            new AuthUserDto(user.Id, user.UserName ?? string.Empty, user.Email, roleNames));
    }

    private async Task<ApplicationUser?> FindUserAsync(string emailOrUsername, CancellationToken cancellationToken)
    {
        var login = emailOrUsername.Trim();
        var user = await _userManager.FindByNameAsync(login);
        if (user is not null)
        {
            return user;
        }

        return await _userManager.FindByEmailAsync(login);
    }

    private bool TryValidateRefreshToken(string refreshToken, out ClaimsPrincipal principal)
    {
        try
        {
            principal = TokenHandler.ValidateToken(refreshToken, _tokenValidationParameters, out var validatedToken);
            if (validatedToken is not JwtSecurityToken)
            {
                return false;
            }

            var tokenType = principal.FindFirstValue(JwtRegisteredClaimNames.Typ);
            return string.Equals(tokenType, JwtTokenTypes.Refresh, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            principal = new ClaimsPrincipal();
            return false;
        }
    }

    private static TokenValidationParameters CreateTokenValidationParameters(JwtOptions jwtOptions, JwtKeys jwtKeys)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKeys.RefreshKey));

        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero
        };
    }

    private static string? GetSubjectId(ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
