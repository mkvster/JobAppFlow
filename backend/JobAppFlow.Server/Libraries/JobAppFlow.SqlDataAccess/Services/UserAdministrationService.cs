using JobAppFlow.SqlDataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace JobAppFlow.SqlDataAccess.Services;

public sealed class UserAdministrationService : IUserAdministrationService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserAdministrationService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> AddUserAsync(
        string login,
        string password,
        string email,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = new ApplicationUser
        {
            UserName = login.Trim(),
            Email = email.Trim(),
            LockoutEnabled = true,
        };

        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> RemoveUserAsync(
        string login,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await FindUserByLoginAsync(login);
        if (user is null)
        {
            return IdentityResult.Failed(CreateUserNotFoundError(login));
        }

        return await _userManager.DeleteAsync(user);
    }

    public async Task<IdentityResult> BanUserAsync(
        string login,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await FindUserByLoginAsync(login);
        if (user is null)
        {
            return IdentityResult.Failed(CreateUserNotFoundError(login));
        }

        var lockoutEndDate = DateTimeOffset.MaxValue;
        var lockoutEnabledResult = await _userManager.SetLockoutEnabledAsync(user, true);
        if (!lockoutEnabledResult.Succeeded)
        {
            return lockoutEnabledResult;
        }

        return await _userManager.SetLockoutEndDateAsync(user, lockoutEndDate);
    }

    public async Task<IdentityResult> UnbanUserAsync(
        string login,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await FindUserByLoginAsync(login);
        if (user is null)
        {
            return IdentityResult.Failed(CreateUserNotFoundError(login));
        }

        var lockoutEnabledResult = await _userManager.SetLockoutEnabledAsync(user, true);
        if (!lockoutEnabledResult.Succeeded)
        {
            return lockoutEnabledResult;
        }

        return await _userManager.SetLockoutEndDateAsync(user, null);
    }

    private async Task<ApplicationUser?> FindUserByLoginAsync(string login)
    {
        return await _userManager.FindByNameAsync(login.Trim());
    }

    private static IdentityError CreateUserNotFoundError(string login)
    {
        return new IdentityError
        {
            Code = "UserNotFound",
            Description = $"User '{login}' was not found.",
        };
    }
}
