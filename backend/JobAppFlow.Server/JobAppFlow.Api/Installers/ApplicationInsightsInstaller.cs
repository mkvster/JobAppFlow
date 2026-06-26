using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace JobAppFlow.Api.Installers;

public sealed class ApplicationInsightsInstaller : IFeatureInstaller
{
    private const string ConnectionStringKey = "APPLICATIONINSIGHTS_CONNECTION_STRING";

    public void ConfigureBuilder(IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var connectionString = builder.Configuration[ConnectionStringKey];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return;
        }

        builder.Services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString = connectionString;
        });
    }

    public void ConfigureApp(IApplicationBuilder app)
    {
    }
}
