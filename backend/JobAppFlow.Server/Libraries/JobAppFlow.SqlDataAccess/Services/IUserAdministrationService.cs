using JobAppFlow.SqlDataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace JobAppFlow.SqlDataAccess.Services;

public interface IUserAdministrationService
{
    Task<IdentityResult> AddUserAsync(
        string login,
        string password,
        string email,
        CancellationToken cancellationToken = default);

    Task<IdentityResult> RemoveUserAsync(
        string login,
        CancellationToken cancellationToken = default);

    Task<IdentityResult> BanUserAsync(
        string login,
        CancellationToken cancellationToken = default);

    Task<IdentityResult> UnbanUserAsync(
        string login,
        CancellationToken cancellationToken = default);

    Task<IdentityResult> AddRoleAsync(
        string roleName,
        CancellationToken cancellationToken = default);

    Task<IdentityResult> RemoveRoleAsync(
        string roleName,
        CancellationToken cancellationToken = default);

    Task<IdentityResult> AddUserRoleAsync(
        string login,
        string roleName,
        CancellationToken cancellationToken = default);

    Task<IdentityResult> RemoveUserRoleAsync(
        string login,
        string roleName,
        CancellationToken cancellationToken = default);
}
