using System.Text;
using JobAppFlow.Api.Extensions;
using JobAppFlow.Api.Models.Options;
using JobAppFlow.Api.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace JobAppFlow.Api.Installers;

public sealed class JwtAuthInstaller : IFeatureInstaller
{
    private const string JwtConfigSectionName = "Jwt";

    public void ConfigureBuilder(IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var jwtOptions = builder.Configuration.ReadSection<JwtOptions>(JwtConfigSectionName);
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));

        builder.Services.AddSingleton(jwtOptions);
        builder.Services.AddScoped<IAccessTokenGenerator, JwtAccessTokenGenerator>();
        builder.Services.AddScoped<IRefreshTokenJwtGenerator, JwtRefreshTokenGenerator>();

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddAuthorization();
    }

    public void ConfigureApp(IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
