namespace JobAppFlow.Api.Services.Authentication;

public sealed record LoginAttemptThrottle(
    int FailedAttempts,
    TimeSpan Delay,
    bool IsLockedOut,
    TimeSpan RetryAfter)
{
    public ValueTask WaitAsync(CancellationToken cancellationToken)
    {
        if (Delay <= TimeSpan.Zero)
        {
            return ValueTask.CompletedTask;
        }

        return new ValueTask(Task.Delay(Delay, cancellationToken));
    }
}
