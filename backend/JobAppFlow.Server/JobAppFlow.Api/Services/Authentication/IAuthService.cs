using JobAppFlow.SqlDataAccess.Models;
using System.Security.Claims;

namespace JobAppFlow.Api.Services.Authentication;

public interface IAuthService
{
    Task<AuthSessionResult?> LoginAsync(string emailOrUsername, string password, CancellationToken cancellationToken = default);

    Task<AuthSessionResult?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);

    Task LogoutAsync(string? refreshToken, CancellationToken cancellationToken = default);

    Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}
