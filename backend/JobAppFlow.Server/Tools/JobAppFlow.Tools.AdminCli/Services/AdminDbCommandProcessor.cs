using JobAppFlow.SqlDataAccess.Services;
using JobAppFlow.Tools.AdminCli.Models;
using Microsoft.AspNetCore.Identity;

namespace JobAppFlow.Tools.AdminCli.Services;

public sealed class AdminDbCommandProcessor : IAdminDbCommandProcessor
{
    private readonly IUserAdministrationService _userAdministrationService;

    public AdminDbCommandProcessor(IUserAdministrationService userAdministrationService)
    {
        _userAdministrationService = userAdministrationService;
    }

    public async Task AddUserAsync(AddUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _userAdministrationService.AddUserAsync(
            request.Login,
            request.Password,
            request.Email,
            cancellationToken);

        WriteResult("add-user", request.Login, result);
    }

    public async Task RemoveUserAsync(RemoveUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _userAdministrationService.RemoveUserAsync(request.Login, cancellationToken);
        WriteResult("remove-user", request.Login, result);
    }

    public async Task BanUserAsync(BanUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _userAdministrationService.BanUserAsync(request.Login, cancellationToken);
        WriteResult("ban-user", request.Login, result);
    }

    public async Task UnbanUserAsync(UnbanUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _userAdministrationService.UnbanUserAsync(request.Login, cancellationToken);
        WriteResult("unban-user", request.Login, result);
    }

    private static void WriteResult(string commandName, string login, IdentityResult result)
    {
        if (result.Succeeded)
        {
            Console.WriteLine($"Admin db {commandName} completed for login={login}.");
            return;
        }

        var errors = string.Join(
            Environment.NewLine,
            result.Errors.Select(error => $"- {error.Code}: {error.Description}"));

        throw new InvalidOperationException(
            $"Admin db {commandName} failed for login={login}.{Environment.NewLine}{errors}");
    }
}
