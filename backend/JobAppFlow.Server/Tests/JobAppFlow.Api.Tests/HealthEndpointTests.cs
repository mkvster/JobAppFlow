using System.Net;

namespace JobAppFlow.Api.Tests;

[Collection(IntegrationTestCollectionDefinition.Name)]
public sealed class HealthEndpointTests
{
    private readonly HttpClient _client;

    public HealthEndpointTests(IntegrationTestFixture fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }
}
