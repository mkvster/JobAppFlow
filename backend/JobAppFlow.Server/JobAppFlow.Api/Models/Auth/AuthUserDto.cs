namespace JobAppFlow.Api.Models.Auth;

public sealed record AuthUserDto(Guid Id, string UserName, string? Email, string[] Roles);
