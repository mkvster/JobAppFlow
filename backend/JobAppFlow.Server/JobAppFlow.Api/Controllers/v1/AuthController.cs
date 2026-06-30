using JobAppFlow.Api.Extensions;
using JobAppFlow.Api.Logging;
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
    private readonly ILoginAttemptProtectionService _loginAttemptProtectionService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ILoginAttemptProtectionService loginAttemptProtectionService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _loginAttemptProtectionService = loginAttemptProtectionService;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthSessionDto>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var loginKey = NormalizeLoginKey(request.EmailOrUsername);
        var hasNoThrottle = await CheckNoThrottle(loginKey, cancellationToken);
        if (!hasNoThrottle)
        {
            return new StatusCodeResult(StatusCodes.Status429TooManyRequests);
        }

        var session = await _authService.LoginAsync(
            request.EmailOrUsername,
            request.Password,
            cancellationToken);

        if (session is null)
        {
            var failure = _loginAttemptProtectionService.RegisterFailure(loginKey);
            _logger.LogLoginAttemptFailed(loginKey, HttpContext, failure);

            await failure.WaitAsync(cancellationToken);

            if (failure.IsLockedOut)
            {
                Response.SetRetryAfterHeader(failure.RetryAfter);
                return new StatusCodeResult(StatusCodes.Status429TooManyRequests);
            }

            return new UnauthorizedResult();
        }

        _loginAttemptProtectionService.RegisterSuccess(loginKey);
        SetRefreshTokenCookie(session);
        return Ok(new AuthSessionDto(session.AccessToken, session.User));
    }

    [AllowAnonymous]
    [HttpPost("demo")]
    public async Task<ActionResult<AuthSessionDto>> Demo(CancellationToken cancellationToken)
    {
        var session = await _authService.LoginDemoAsync(cancellationToken);
        if (session is null)
        {
            return Unauthorized();
        }

        SetRefreshTokenCookie(session);
        return Ok(new AuthSessionDto(session.AccessToken, session.User));
    }

    private async Task<bool> CheckNoThrottle(string loginKey, CancellationToken cancellationToken)
    {
        var attempt = _loginAttemptProtectionService.GetThrottle(loginKey);
        if (attempt.IsLockedOut)
        {
            Response.SetRetryAfterHeader(attempt.RetryAfter);
            _logger.LogLoginAttemptLockedOut(loginKey, HttpContext, attempt);
            return false;
        }

        await attempt.WaitAsync(cancellationToken);

        return true;
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

        var roles = await _authService.GetCurrentUserRolesAsync(User, cancellationToken);
        return Ok(new AuthUserDto(user.Id, user.UserName ?? string.Empty, user.Email, roles));
    }

    private void SetRefreshTokenCookie(AuthSessionResult session)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = Request.IsHttps
                ? Microsoft.AspNetCore.Http.SameSiteMode.None
                : Microsoft.AspNetCore.Http.SameSiteMode.Lax,
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

    private static string NormalizeLoginKey(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Trim().ToUpperInvariant();
    }
}
