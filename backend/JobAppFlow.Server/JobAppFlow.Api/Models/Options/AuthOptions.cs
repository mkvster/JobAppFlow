namespace JobAppFlow.Api.Models.Options;

public sealed class AuthOptions
{
    public int FailureHistoryWindowMinutes { get; init; } = 15;

    public int SoftBackoffStartFailureCount { get; init; } = 1;

    public int SoftBackoffInitialDelayMilliseconds { get; init; } = 250;

    public double SoftBackoffMultiplier { get; init; } = 2.0;

    public int SoftBackoffMaxDelayMilliseconds { get; init; } = 4_000;

    public int LockoutAfterFailedAttempts { get; init; } = 5;

    public int LockoutDurationMinutes { get; init; } = 15;
}
