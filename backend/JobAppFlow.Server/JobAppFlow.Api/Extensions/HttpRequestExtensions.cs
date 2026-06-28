using Microsoft.Net.Http.Headers;

namespace JobAppFlow.Api.Extensions;

public static class HttpRequestExtensions
{
    public static string GetUserAgent(this HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return request.Headers.UserAgent.ToString();
    }
}
