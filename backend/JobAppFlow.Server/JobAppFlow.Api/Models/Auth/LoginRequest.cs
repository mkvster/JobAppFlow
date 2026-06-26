namespace JobAppFlow.Api.Models.Auth;

public sealed record LoginRequest(string EmailOrUsername, string Password);
