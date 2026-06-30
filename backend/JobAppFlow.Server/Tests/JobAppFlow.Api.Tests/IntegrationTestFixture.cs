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
    private const string DemoUserName = "demo";
    private const string DemoUserEmail = "demo@example.com";
    private const string DemoUserPassword = "Demo123!";

    public TestWebApplicationFactory Factory { get; } = new();

    public async Task InitializeAsync()
    {
        using var scope = Factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<JobAppFlowDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        await SeedUserAsync(userManager, TestUserName, TestUserEmail, TestUserPassword);
        await SeedDemoUserAsync(userManager, roleManager);
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

    private static async Task SeedUserAsync(
        UserManager<ApplicationUser> userManager,
        string userName,
        string email,
        string password,
        string? role = null)
    {
        var existingUser = await userManager.FindByNameAsync(userName);
        if (existingUser is not null)
        {
            return;
        }

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            LockoutEnabled = true
        };

        var result = await userManager.CreateAsync(user, password);
        Assert.True(result.Succeeded, string.Join(", ", result.Errors.Select(error => error.Description)));

        if (!string.IsNullOrWhiteSpace(role))
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);
            Assert.True(roleResult.Succeeded, string.Join(", ", roleResult.Errors.Select(error => error.Description)));
        }
    }

    private static async Task SeedDemoUserAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync(ApplicationRoleNames.Demo))
        {
            var roleResult = await roleManager.CreateAsync(new ApplicationRole { Name = ApplicationRoleNames.Demo });
            Assert.True(roleResult.Succeeded, string.Join(", ", roleResult.Errors.Select(error => error.Description)));
        }

        var existingUser = await userManager.FindByNameAsync(DemoUserName);
        if (existingUser is not null)
        {
            if (!await userManager.IsInRoleAsync(existingUser, ApplicationRoleNames.Demo))
            {
                var addRoleResult = await userManager.AddToRoleAsync(existingUser, ApplicationRoleNames.Demo);
                Assert.True(addRoleResult.Succeeded, string.Join(", ", addRoleResult.Errors.Select(error => error.Description)));
            }

            return;
        }

        var user = new ApplicationUser
        {
            UserName = DemoUserName,
            Email = DemoUserEmail,
            LockoutEnabled = true
        };

        var result = await userManager.CreateAsync(user, DemoUserPassword);
        Assert.True(result.Succeeded, string.Join(", ", result.Errors.Select(error => error.Description)));

        var roleAssignmentResult = await userManager.AddToRoleAsync(user, ApplicationRoleNames.Demo);
        Assert.True(roleAssignmentResult.Succeeded, string.Join(", ", roleAssignmentResult.Errors.Select(error => error.Description)));
    }
}
