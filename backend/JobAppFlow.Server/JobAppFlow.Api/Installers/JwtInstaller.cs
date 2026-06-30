using System.Text;
using JobAppFlow.Api.Extensions;
using JobAppFlow.Api.Constants;
using JobAppFlow.Api.Models.Options;
using JobAppFlow.Api.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace JobAppFlow.Api.Installers;

public sealed class JwtInstaller : IFeatureInstaller
{
    private const string JwtConfigSectionName = "Jwt";
    private const string JwtKeysConfigSectionName = "JwtKeys";

    public void ConfigureBuilder(IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var jwtOptions = builder.Configuration.ReadSection<JwtOptions>(JwtConfigSectionName);
        var jwtKeys = builder.Configuration.ReadSection<JwtKeys>(JwtKeysConfigSectionName);
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKeys.AccessKey));

        builder.Services.AddSingleton(jwtOptions);
        builder.Services.AddSingleton(jwtKeys);
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
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var tokenType = context.Principal?.FindFirst(JwtRegisteredClaimNames.Typ)?.Value;
                        if (string.Equals(tokenType, JwtTokenTypes.Refresh, StringComparison.OrdinalIgnoreCase))
                        {
                            context.Fail("Refresh tokens are not valid for access authorization.");
                        }

                        return Task.CompletedTask;
                    }
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
