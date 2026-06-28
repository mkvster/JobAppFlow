using System.Collections.Concurrent;
using JobAppFlow.Api.Models.Options;

namespace JobAppFlow.Api.Services.Authentication;

public sealed class InMemoryLoginAttemptProtectionService : ILoginAttemptProtectionService
{
    private readonly ConcurrentDictionary<string, LoginAttemptState> _states = new(StringComparer.OrdinalIgnoreCase);
    private readonly AuthOptions _options;
    private readonly TimeProvider _timeProvider;

    public InMemoryLoginAttemptProtectionService(
        AuthOptions options,
        TimeProvider timeProvider)
    {
        _options = options;
        _timeProvider = timeProvider;
    }

    public LoginAttemptThrottle GetThrottle(string loginKey)
    {
        var normalizedKey = NormalizeKey(loginKey);
        var now = _timeProvider.GetUtcNow();
        var state = _states.GetOrAdd(normalizedKey, static _ => new LoginAttemptState());

        lock (state)
        {
            ResetIfExpired(state, now);

            if (state.LockedUntilUtc is not null && state.LockedUntilUtc > now)
            {
                return new LoginAttemptThrottle(
                    state.FailedAttempts,
                    TimeSpan.Zero,
                    true,
                    state.LockedUntilUtc.Value - now);
            }

            var delay = CalculateDelay(state.FailedAttempts);
            return new LoginAttemptThrottle(
                state.FailedAttempts,
                delay,
                false,
                TimeSpan.Zero);
        }
    }

    public LoginAttemptThrottle RegisterFailure(string loginKey)
    {
        var normalizedKey = NormalizeKey(loginKey);
        var now = _timeProvider.GetUtcNow();
        var state = _states.GetOrAdd(normalizedKey, static _ => new LoginAttemptState());

        lock (state)
        {
            ResetIfExpired(state, now);

            state.FailedAttempts++;
            state.LastFailureUtc = now;

            if (state.FailedAttempts >= _options.LockoutAfterFailedAttempts)
            {
                state.LockedUntilUtc = now.AddMinutes(_options.LockoutDurationMinutes);
                return new LoginAttemptThrottle(
                    state.FailedAttempts,
                    TimeSpan.Zero,
                    true,
                    state.LockedUntilUtc.Value - now);
            }

            return new LoginAttemptThrottle(
                state.FailedAttempts,
                CalculateDelay(state.FailedAttempts),
                false,
                TimeSpan.Zero);
        }
    }

    public void RegisterSuccess(string loginKey)
    {
        var normalizedKey = NormalizeKey(loginKey);
        _states.TryRemove(normalizedKey, out _);
    }

    private TimeSpan CalculateDelay(int failureCount)
    {
        if (failureCount < _options.SoftBackoffStartFailureCount)
        {
            return TimeSpan.Zero;
        }

        var backoffStep = failureCount - _options.SoftBackoffStartFailureCount;
        var delayMilliseconds = _options.SoftBackoffInitialDelayMilliseconds *
            Math.Pow(_options.SoftBackoffMultiplier, backoffStep);
        var clampedDelay = Math.Min(delayMilliseconds, _options.SoftBackoffMaxDelayMilliseconds);

        return TimeSpan.FromMilliseconds(clampedDelay);
    }

    private void ResetIfExpired(LoginAttemptState state, DateTimeOffset now)
    {
        if (state.LockedUntilUtc is not null && state.LockedUntilUtc <= now)
        {
            state.Reset();
            return;
        }

        if (state.LastFailureUtc is not null &&
            now - state.LastFailureUtc.Value > TimeSpan.FromMinutes(_options.FailureHistoryWindowMinutes))
        {
            state.Reset();
        }
    }

    private static string NormalizeKey(string loginKey)
    {
        return string.IsNullOrWhiteSpace(loginKey)
            ? string.Empty
            : loginKey.Trim().ToUpperInvariant();
    }

    private sealed class LoginAttemptState
    {
        public int FailedAttempts { get; set; }

        public DateTimeOffset? LastFailureUtc { get; set; }

        public DateTimeOffset? LockedUntilUtc { get; set; }

        public void Reset()
        {
            FailedAttempts = 0;
            LastFailureUtc = null;
            LockedUntilUtc = null;
        }
    }
}
