using JobAppFlow.SqlDataAccess.Models;
using JobAppFlow.SqlDataAccess.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JobAppFlow.Api.Tests;

[Collection(IntegrationTestCollectionDefinition.Name)]
public sealed class RoleAdministrationServiceTests
{
    private const string TestUserName = "test-user";

    private readonly IntegrationTestFixture _fixture;

    public RoleAdministrationServiceTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AddRoleAsync_CreatesRole()
    {
        using var scope = _fixture.Factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IUserAdministrationService>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        var result = await service.AddRoleAsync("reviewer");

        Assert.True(result.Succeeded, string.Join(", ", result.Errors.Select(error => error.Description)));
        Assert.NotNull(await roleManager.FindByNameAsync("reviewer"));
    }

    [Fact]
    public async Task AddUserRoleAsync_AssignsRoleToUser()
    {
        using var scope = _fixture.Factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IUserAdministrationService>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await service.AddRoleAsync("reviewer");
        var result = await service.AddUserRoleAsync(TestUserName, "reviewer");

        Assert.True(result.Succeeded, string.Join(", ", result.Errors.Select(error => error.Description)));

        var user = await userManager.FindByNameAsync(TestUserName);
        Assert.NotNull(user);
        Assert.True(await userManager.IsInRoleAsync(user, "reviewer"));
    }

    [Fact]
    public async Task RemoveUserRoleAsync_RemovesRoleFromUser()
    {
        using var scope = _fixture.Factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IUserAdministrationService>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await service.AddRoleAsync("reviewer");
        await service.AddUserRoleAsync(TestUserName, "reviewer");

        var result = await service.RemoveUserRoleAsync(TestUserName, "reviewer");

        Assert.True(result.Succeeded, string.Join(", ", result.Errors.Select(error => error.Description)));

        var user = await userManager.FindByNameAsync(TestUserName);
        Assert.NotNull(user);
        Assert.False(await userManager.IsInRoleAsync(user, "reviewer"));
    }

    [Fact]
    public async Task RemoveRoleAsync_DeletesRole()
    {
        using var scope = _fixture.Factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IUserAdministrationService>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        await service.AddRoleAsync("reviewer");
        var result = await service.RemoveRoleAsync("reviewer");

        Assert.True(result.Succeeded, string.Join(", ", result.Errors.Select(error => error.Description)));
        Assert.Null(await roleManager.FindByNameAsync("reviewer"));
    }
}
