using JobAppFlow.Api.Models.Auth;
using JobAppFlow.Api.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobAppFlow.Api.Controllers.v1;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    private const string RefreshTokenCookieName = "jobappflow_refresh_token";
    private const string CookiePath = "/api/v1/auth";

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthSessionDto>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = await _authService.LoginAsync(
            request.EmailOrUsername,
            request.Password,
            cancellationToken);

        if (session is null)
        {
            return Unauthorized();
        }

        SetRefreshTokenCookie(session);
        return Ok(new AuthSessionDto(session.AccessToken, session.User));
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthSessionDto>> Refresh(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken))
        {
            return Unauthorized();
        }

        var session = await _authService.RefreshAsync(refreshToken, cancellationToken);
        if (session is null)
        {
            return Unauthorized();
        }

        SetRefreshTokenCookie(session);
        return Ok(new AuthSessionDto(session.AccessToken, session.User));
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        Request.Cookies.TryGetValue(RefreshTokenCookieName, out var refreshToken);

        await _authService.LogoutAsync(refreshToken, cancellationToken);
        DeleteRefreshTokenCookie();
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<AuthUserDto>> Me(CancellationToken cancellationToken)
    {
        var user = await _authService.GetCurrentUserAsync(User, cancellationToken);
        if (user is null)
        {
            return Unauthorized();
        }

        return Ok(new AuthUserDto(user.Id, user.UserName ?? string.Empty, user.Email));
    }

    private void SetRefreshTokenCookie(AuthSessionResult session)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = Request.IsHttps ? SameSiteMode.None : SameSiteMode.Lax,
            Expires = session.RefreshTokenExpiresAtUtc.UtcDateTime,
            Path = CookiePath
        };

        Response.Cookies.Append(
            RefreshTokenCookieName,
            session.RefreshToken,
            cookieOptions);
    }

    private void DeleteRefreshTokenCookie()
    {
        Response.Cookies.Delete(
            RefreshTokenCookieName,
            new CookieOptions
            {
                Path = CookiePath
            });
    }
}
