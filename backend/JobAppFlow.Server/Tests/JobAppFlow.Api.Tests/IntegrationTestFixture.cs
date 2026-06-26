using JobAppFlow.SqlDataAccess.Data;
using JobAppFlow.SqlDataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JobAppFlow.Api.Tests;

public sealed class IntegrationTestFixture : IAsyncLifetime
{
    private const string TestUserName = "test-user";
    private const string TestUserEmail = "test@example.com";
    private const string TestUserPassword = "Password123!";

    public TestWebApplicationFactory Factory { get; } = new();

    public async Task InitializeAsync()
    {
        using var scope = Factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<JobAppFlowDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        await SeedUserAsync(userManager);
    }

    public Task DisposeAsync()
    {
        Factory.Dispose();
        return Task.CompletedTask;
    }

    public HttpClient CreateClient()
    {
        return Factory.CreateClient();
    }

    private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
    {
        if (await userManager.FindByNameAsync(TestUserName) is not null)
        {
            return;
        }

        var user = new ApplicationUser
        {
            UserName = TestUserName,
            Email = TestUserEmail,
            LockoutEnabled = true
        };

        var result = await userManager.CreateAsync(user, TestUserPassword);
        Assert.True(result.Succeeded, string.Join(", ", result.Errors.Select(error => error.Description)));
    }
}
