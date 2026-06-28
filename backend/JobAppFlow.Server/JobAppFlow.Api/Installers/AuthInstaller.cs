using System.Text;
using JobAppFlow.Api.Extensions;
using JobAppFlow.Api.Constants;
using JobAppFlow.Api.Models.Options;
using JobAppFlow.Api.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace JobAppFlow.Api.Installers;

public sealed class AuthInstaller : IFeatureInstaller
{
    private const string AuthConfigSectionName = "Auth";

    public void ConfigureBuilder(IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var authOptions = builder.Configuration.ReadSection<AuthOptions>(AuthConfigSectionName);
        
        builder.Services.AddSingleton(authOptions);
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<ILoginAttemptProtectionService, InMemoryLoginAttemptProtectionService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
    }

    public void ConfigureApp(IApplicationBuilder app)
    {

    }
}
