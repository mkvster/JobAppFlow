namespace JobAppFlow.Api.Extensions;

public static class HttpContextExtensions
{
    public static string GetClientIpAddress(this HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}
