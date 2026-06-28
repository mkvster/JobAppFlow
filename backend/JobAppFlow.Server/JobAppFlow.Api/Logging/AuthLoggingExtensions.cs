using JobAppFlow.Api.Extensions;
using JobAppFlow.Api.Services.Authentication;
using Microsoft.Extensions.Logging;

namespace JobAppFlow.Api.Logging;

public static class AuthLoggingExtensions
{
    private static readonly EventId LoginAttemptFailedEvent = new(1000, nameof(LogLoginAttemptFailed));
    private static readonly EventId LoginAttemptLockedOutEvent = new(1001, nameof(LogLoginAttemptLockedOut));

    public static void LogLoginAttemptFailed(
        this ILogger logger,
        string loginKey,
        string clientIp,
        string userAgent,
        int failedAttempts,
        bool lockedOut)
    {
        ArgumentNullException.ThrowIfNull(logger);

        logger.LogWarning(
            LoginAttemptFailedEvent,
            "Login failed for {LoginKey} from {ClientIp} with user agent {UserAgent}. FailedAttempts={FailedAttempts}, LockedOut={LockedOut}",
            loginKey,
            clientIp,
            userAgent,
            failedAttempts,
            lockedOut);
    }

    public static void LogLoginAttemptFailed(
        this ILogger logger,
        string loginKey,
        HttpContext httpContext,
        LoginAttemptThrottle throttle)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var clientIp = httpContext.GetClientIpAddress();
        var userAgent = httpContext.Request.GetUserAgent();
        var failedAttempts = throttle.FailedAttempts;
        var lockedOut = throttle.IsLockedOut;

        LogLoginAttemptFailed(
            logger,
            loginKey,
            clientIp,
            userAgent,
            failedAttempts,
            lockedOut);
    }

    public static void LogLoginAttemptLockedOut(
        this ILogger logger,
        string loginKey,
        string clientIp,
        string userAgent,
        int failedAttempts,
        TimeSpan retryAfter)
    {
        ArgumentNullException.ThrowIfNull(logger);

        logger.LogWarning(
            LoginAttemptLockedOutEvent,
            "Login locked out for {LoginKey} from {ClientIp} with user agent {UserAgent}. FailedAttempts={FailedAttempts}, RetryAfterSeconds={RetryAfterSeconds}",
            loginKey,
            clientIp,
            userAgent,
            failedAttempts,
            (int)Math.Ceiling(retryAfter.TotalSeconds));
    }

    public static void LogLoginAttemptLockedOut(
        this ILogger logger,
        string loginKey,
        HttpContext httpContext,
        LoginAttemptThrottle throttle)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var clientIp = httpContext.GetClientIpAddress();
        var userAgent = httpContext.Request.GetUserAgent();
        var failedAttempts = throttle.FailedAttempts;
        var retryAfter = throttle.RetryAfter;

        LogLoginAttemptLockedOut(
            logger,
            loginKey,
            clientIp,
            userAgent,
            failedAttempts,
            retryAfter
        );
    }
}
