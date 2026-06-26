using JobAppFlow.SqlDataAccess.Models;

namespace JobAppFlow.Api.Services.Authentication;

public interface IRefreshTokenJwtGenerator
{
    string Generate(ApplicationUser user);
}
