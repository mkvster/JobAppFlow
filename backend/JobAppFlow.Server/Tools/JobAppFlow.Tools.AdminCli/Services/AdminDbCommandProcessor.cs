using JobAppFlow.Tools.AdminCli.Models;

namespace JobAppFlow.Tools.AdminCli.Services;

public sealed class AdminDbCommandProcessor : IAdminDbCommandProcessor
{
    public Task AddUserAsync(AddUserRequest request, CancellationToken cancellationToken = default)
    {
        WritePreview("add-user", $"login={request.Login}; email={request.Email}");
        return Task.CompletedTask;
    }

    public Task RemoveUserAsync(RemoveUserRequest request, CancellationToken cancellationToken = default)
    {
        WritePreview("remove-user", $"login={request.Login}");
        return Task.CompletedTask;
    }

    public Task BanUserAsync(BanUserRequest request, CancellationToken cancellationToken = default)
    {
        WritePreview("ban-user", $"login={request.Login}");
        return Task.CompletedTask;
    }

    public Task UnbanUserAsync(UnbanUserRequest request, CancellationToken cancellationToken = default)
    {
        WritePreview("unban-user", $"login={request.Login}");
        return Task.CompletedTask;
    }

    private static void WritePreview(string commandName, string details)
    {
        Console.WriteLine($"Parsed db {commandName}: {details}");
        Console.WriteLine("ORM backend is not wired yet. This is a parser/dispatch scaffold.");
    }
}
