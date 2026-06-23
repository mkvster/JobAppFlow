namespace JobAppFlow.Tools.AdminCli.Models;

public sealed record AddUserRequest(string Login, string Password, string Email);

public sealed record RemoveUserRequest(string Login);

public sealed record BanUserRequest(string Login);

public sealed record UnbanUserRequest(string Login);
