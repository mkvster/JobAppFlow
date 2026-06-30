using JobAppFlow.Tools.AdminCli.Models;

namespace JobAppFlow.Tools.AdminCli.Services;

public interface IAdminDbCommandProcessor
{
    Task AddUserAsync(AddUserRequest request, CancellationToken cancellationToken = default);

    Task RemoveUserAsync(RemoveUserRequest request, CancellationToken cancellationToken = default);

    Task BanUserAsync(BanUserRequest request, CancellationToken cancellationToken = default);

    Task UnbanUserAsync(UnbanUserRequest request, CancellationToken cancellationToken = default);

    Task AddRoleAsync(AddRoleRequest request, CancellationToken cancellationToken = default);

    Task RemoveRoleAsync(RemoveRoleRequest request, CancellationToken cancellationToken = default);

    Task AddUserRoleAsync(AddUserRoleRequest request, CancellationToken cancellationToken = default);

    Task RemoveUserRoleAsync(RemoveUserRoleRequest request, CancellationToken cancellationToken = default);
}
