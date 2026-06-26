namespace JobAppFlow.Api.Models.Options;

public sealed class JwtKeys
{
    public string AccessKey { get; init; } = string.Empty;

    public string RefreshKey { get; init; } = string.Empty;
}
