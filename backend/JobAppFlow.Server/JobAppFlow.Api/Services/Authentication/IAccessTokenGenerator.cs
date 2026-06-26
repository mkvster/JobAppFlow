using JobAppFlow.SqlDataAccess.Models;

namespace JobAppFlow.Api.Services.Authentication;

public interface IAccessTokenGenerator
{
    string Generate(ApplicationUser user);
}
