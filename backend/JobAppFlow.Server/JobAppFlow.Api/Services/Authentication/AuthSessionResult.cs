using JobAppFlow.Api.Models.Auth;

namespace JobAppFlow.Api.Services.Authentication;

public sealed record AuthSessionResult(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAtUtc,
    AuthUserDto User);
