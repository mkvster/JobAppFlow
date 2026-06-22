using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace JobAppFlow.Api.Tests;

public class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>> {
    private readonly HttpClient _client;

    public HealthEndpointTests(WebApplicationFactory<Program> factory) {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ReturnsSuccess() {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }
}
