using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace JobAppFlow.Api.Tests;

[Collection(IntegrationTestCollectionDefinition.Name)]
public sealed class AuthEndpointTests
{
    private sealed record LoginRequestPayload(string emailOrUsername, string password);
    private static readonly LoginRequestPayload LoginRequest = new("test-user", "Password123!");
    
    private readonly IntegrationTestFixture _fixture;

    public AuthEndpointTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Login_ReturnsAccessToken_AndSetsRefreshCookie()
    {
        using var client = _fixture.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/auth/login", LoginRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Headers.TryGetValues("Set-Cookie", out var setCookieValues));
        Assert.Contains(setCookieValues, value => value.Contains("jobappflow_refresh_token=", StringComparison.OrdinalIgnoreCase));

        var payload = await ReadJsonAsync(response);
        Assert.Equal("test-user", payload.RootElement.GetProperty("user").GetProperty("userName").GetString());
        Assert.False(string.IsNullOrWhiteSpace(payload.RootElement.GetProperty("accessToken").GetString()));
    }

    [Fact]
    public async Task Refresh_RotatesToken_AndReturnsNewAccessToken()
    {
        using var client = _fixture.CreateClient();

        var loginResponse = await client.PostAsJsonAsync("/api/v1/auth/login", LoginRequest);

        var refreshCookie = GetRefreshCookie(loginResponse);
        client.DefaultRequestHeaders.Add("Cookie", refreshCookie);

        var refreshResponse = await client.PostAsync("/api/v1/auth/refresh", null);

        Assert.Equal(HttpStatusCode.OK, refreshResponse.StatusCode);
        Assert.True(refreshResponse.Headers.TryGetValues("Set-Cookie", out var setCookieValues));
        Assert.Contains(setCookieValues, value => value.Contains("jobappflow_refresh_token=", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Me_ReturnsCurrentUser_WhenAuthorized()
    {
        using var client = _fixture.CreateClient();

        var loginResponse = await client.PostAsJsonAsync("/api/v1/auth/login", LoginRequest);

        var payload = await ReadJsonAsync(loginResponse);
        var accessToken = payload.RootElement.GetProperty("accessToken").GetString();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var meResponse = await client.GetAsync("/api/v1/auth/me");

        Assert.Equal(HttpStatusCode.OK, meResponse.StatusCode);
        var mePayload = await ReadJsonAsync(meResponse);
        Assert.Equal("test-user", mePayload.RootElement.GetProperty("userName").GetString());
    }

    [Fact]
    public async Task Logout_ReturnsNoContent()
    {
        using var client = _fixture.CreateClient();

        var loginResponse = await client.PostAsJsonAsync("/api/v1/auth/login", LoginRequest);

        client.DefaultRequestHeaders.Add("Cookie", GetRefreshCookie(loginResponse));

        var logoutResponse = await client.PostAsync("/api/v1/auth/logout", null);

        Assert.Equal(HttpStatusCode.NoContent, logoutResponse.StatusCode);
    }

    private static string GetRefreshCookie(HttpResponseMessage response)
    {
        var cookie = response.Headers.GetValues("Set-Cookie")
            .First(value => value.Contains("jobappflow_refresh_token=", StringComparison.OrdinalIgnoreCase));

        return cookie.Split(';', 2)[0];
    }

    private static async Task<JsonDocument> ReadJsonAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(content);
    }
}
