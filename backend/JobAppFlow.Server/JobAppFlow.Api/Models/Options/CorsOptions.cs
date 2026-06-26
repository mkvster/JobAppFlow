namespace JobAppFlow.Api.Models.Options;

public sealed class CorsOptions
{
    public string[] AllowedOrigins { get; init; } = [];
}
