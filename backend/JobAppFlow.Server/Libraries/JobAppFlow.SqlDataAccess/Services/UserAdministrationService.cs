using JobAppFlow.SqlDataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace JobAppFlow.SqlDataAccess.Services;

public sealed class UserAdministrationService : IUserAdministrationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UserAdministrationService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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

    public async Task<IdentityResult> AddRoleAsync(
        string roleName,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var normalizedRoleName = NormalizeRoleName(roleName);
        if (string.IsNullOrWhiteSpace(normalizedRoleName))
        {
            return IdentityResult.Failed(CreateRoleNameInvalidError(roleName));
        }

        var role = await _roleManager.FindByNameAsync(normalizedRoleName);
        if (role is not null)
        {
            return IdentityResult.Success;
        }

        return await _roleManager.CreateAsync(new ApplicationRole
        {
            Name = normalizedRoleName
        });
    }

    public async Task<IdentityResult> RemoveRoleAsync(
        string roleName,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var role = await FindRoleByNameAsync(roleName);
        if (role is null)
        {
            return IdentityResult.Success;
        }

        return await _roleManager.DeleteAsync(role);
    }

    public async Task<IdentityResult> AddUserRoleAsync(
        string login,
        string roleName,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await FindUserByLoginAsync(login);
        if (user is null)
        {
            return IdentityResult.Failed(CreateUserNotFoundError(login));
        }

        var role = await FindRoleByNameAsync(roleName);
        if (role is null)
        {
            return IdentityResult.Failed(CreateRoleNotFoundError(roleName));
        }

        var normalizedRoleName = NormalizeRoleName(role.Name ?? roleName);
        if (await _userManager.IsInRoleAsync(user, normalizedRoleName))
        {
            return IdentityResult.Success;
        }

        return await _userManager.AddToRoleAsync(user, normalizedRoleName);
    }

    public async Task<IdentityResult> RemoveUserRoleAsync(
        string login,
        string roleName,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await FindUserByLoginAsync(login);
        if (user is null)
        {
            return IdentityResult.Failed(CreateUserNotFoundError(login));
        }

        var role = await FindRoleByNameAsync(roleName);
        if (role is null)
        {
            return IdentityResult.Failed(CreateRoleNotFoundError(roleName));
        }

        var normalizedRoleName = NormalizeRoleName(role.Name ?? roleName);
        if (!await _userManager.IsInRoleAsync(user, normalizedRoleName))
        {
            return IdentityResult.Success;
        }

        return await _userManager.RemoveFromRoleAsync(user, normalizedRoleName);
    }

    private async Task<ApplicationUser?> FindUserByLoginAsync(string login)
    {
        return await _userManager.FindByNameAsync(login.Trim());
    }

    private async Task<ApplicationRole?> FindRoleByNameAsync(string roleName)
    {
        return await _roleManager.FindByNameAsync(NormalizeRoleName(roleName));
    }

    private static string NormalizeRoleName(string roleName)
    {
        return string.IsNullOrWhiteSpace(roleName)
            ? string.Empty
            : roleName.Trim();
    }

    private static IdentityError CreateUserNotFoundError(string login)
    {
        return new IdentityError
        {
            Code = "UserNotFound",
            Description = $"User '{login}' was not found.",
        };
    }

    private static IdentityError CreateRoleNotFoundError(string roleName)
    {
        return new IdentityError
        {
            Code = "RoleNotFound",
            Description = $"Role '{roleName}' was not found.",
        };
    }

    private static IdentityError CreateRoleNameInvalidError(string roleName)
    {
        return new IdentityError
        {
            Code = "RoleNameInvalid",
            Description = $"Role name '{roleName}' is invalid.",
        };
    }
}
