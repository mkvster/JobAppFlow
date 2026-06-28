namespace JobAppFlow.Api.Services.Authentication;

public interface ILoginAttemptProtectionService
{
    LoginAttemptThrottle GetThrottle(string loginKey);

    LoginAttemptThrottle RegisterFailure(string loginKey);

    void RegisterSuccess(string loginKey);
}
