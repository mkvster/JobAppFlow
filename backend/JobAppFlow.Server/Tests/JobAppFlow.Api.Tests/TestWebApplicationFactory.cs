using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace JobAppFlow.Api.Tests;

public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
    private readonly string _databasePath = Path.Combine(
        Path.GetTempPath(),
        $"JobAppFlow.Tests.{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("ConnectionStrings:JobAppFlowDb", $"Data Source={_databasePath}");
        builder.UseSetting("Demo:DemoUserLogin", "demo");
        builder.UseSetting("Jwt:Issuer", "JobAppFlow");
        builder.UseSetting("Jwt:Audience", "JobAppFlow");
        builder.UseSetting("Jwt:AccessTokenExpirationMinutes", "60");
        builder.UseSetting("Jwt:RefreshTokenExpirationDays", "30");
        builder.UseSetting("JwtKeys:AccessKey", "JobAppFlow.Test.AccessKey.JobAppFlow.Test.AccessKey");
        builder.UseSetting("JwtKeys:RefreshKey", "JobAppFlow.Test.RefreshKey.JobAppFlow.Test.RefreshKey");
    }

    public new void Dispose()
    {
        base.Dispose();

        try
        {
            if (File.Exists(_databasePath))
            {
                File.Delete(_databasePath);
            }
        }
        catch
        {
            // Best-effort cleanup; the OS can reclaim the temp file later if needed.
        }
    }
}
