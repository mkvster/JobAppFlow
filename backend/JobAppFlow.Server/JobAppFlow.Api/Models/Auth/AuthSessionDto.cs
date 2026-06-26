namespace JobAppFlow.Api.Models.Auth;

public sealed record AuthSessionDto(string AccessToken, AuthUserDto User);
