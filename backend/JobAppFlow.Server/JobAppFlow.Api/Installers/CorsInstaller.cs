using JobAppFlow.Api.Extensions;
using JobAppFlow.Api.Models.Options;

namespace JobAppFlow.Api.Installers;

public sealed class CorsInstaller : IFeatureInstaller
{
    private const string CorsConfigSectionName = "Cors";
    private const string FrontendPolicyName = "Frontend";

    public void ConfigureBuilder(IHostApplicationBuilder builder)
    {
        var corsOptions = builder.Configuration.ReadSection<CorsOptions>(CorsConfigSectionName);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(FrontendPolicyName, policy =>
            {
                policy.WithOrigins(corsOptions.AllowedOrigins)
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    public void ConfigureApp(IApplicationBuilder app)
    {
        app.UseCors(FrontendPolicyName);
    }
}
