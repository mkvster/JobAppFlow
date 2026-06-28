using JobAppFlow.Api.Models.Options;
using JobAppFlow.Api.Services.Authentication;

namespace JobAppFlow.Api.Tests;

public sealed class LoginAttemptProtectionServiceTests
{
    [Fact]
    public void Backoff_Increases_With_Failures_And_Resets_On_Success()
    {
        var clock = new FakeTimeProvider(DateTimeOffset.UtcNow);
        var service = CreateService(clock);

        var initial = service.GetThrottle("demo");
        Assert.Equal(TimeSpan.Zero, initial.Delay);
        Assert.False(initial.IsLockedOut);

        var firstFailure = service.RegisterFailure("demo");
        Assert.Equal(1, firstFailure.FailedAttempts);
        Assert.Equal(TimeSpan.FromMilliseconds(250), firstFailure.Delay);
        Assert.False(firstFailure.IsLockedOut);

        var secondAttempt = service.GetThrottle("demo");
        Assert.Equal(TimeSpan.FromMilliseconds(250), secondAttempt.Delay);

        var secondFailure = service.RegisterFailure("demo");
        Assert.Equal(2, secondFailure.FailedAttempts);
        Assert.Equal(TimeSpan.FromMilliseconds(500), secondFailure.Delay);

        service.RegisterSuccess("demo");

        var afterSuccess = service.GetThrottle("demo");
        Assert.Equal(0, afterSuccess.FailedAttempts);
        Assert.Equal(TimeSpan.Zero, afterSuccess.Delay);
        Assert.False(afterSuccess.IsLockedOut);
    }

    [Fact]
    public void Lockout_Expires_After_Configured_Duration()
    {
        var clock = new FakeTimeProvider(DateTimeOffset.UtcNow);
        var service = CreateService(clock);

        service.RegisterFailure("demo");
        service.RegisterFailure("demo");
        service.RegisterFailure("demo");
        service.RegisterFailure("demo");

        var locked = service.RegisterFailure("demo");
        Assert.True(locked.IsLockedOut);
        Assert.True(locked.RetryAfter > TimeSpan.Zero);

        clock.Advance(TimeSpan.FromMinutes(15).Add(TimeSpan.FromSeconds(1)));

        var afterExpiry = service.GetThrottle("demo");
        Assert.False(afterExpiry.IsLockedOut);
        Assert.Equal(TimeSpan.Zero, afterExpiry.Delay);
        Assert.Equal(0, afterExpiry.FailedAttempts);
    }

    private static InMemoryLoginAttemptProtectionService CreateService(FakeTimeProvider clock)
    {
        return new InMemoryLoginAttemptProtectionService(
            new LoginProtectionOptions(),
            clock);
    }

    private sealed class FakeTimeProvider : TimeProvider
    {
        private DateTimeOffset _utcNow;

        public FakeTimeProvider(DateTimeOffset utcNow)
        {
            _utcNow = utcNow;
        }

        public override DateTimeOffset GetUtcNow() => _utcNow;

        public void Advance(TimeSpan delta)
        {
            _utcNow = _utcNow.Add(delta);
        }

        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;

        public override long GetTimestamp() => 0;

        public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
        {
            throw new NotSupportedException();
        }
    }
}
