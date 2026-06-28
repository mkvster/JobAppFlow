using System.Globalization;
using Microsoft.Net.Http.Headers;

namespace JobAppFlow.Api.Extensions;

public static class HttpResponseExtensions
{
    public static void SetRetryAfterHeader(this HttpResponse response, TimeSpan retryAfter)
    {
        ArgumentNullException.ThrowIfNull(response);

        var seconds = Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));
        response.Headers[HeaderNames.RetryAfter] = seconds.ToString(CultureInfo.InvariantCulture);
    }
}
