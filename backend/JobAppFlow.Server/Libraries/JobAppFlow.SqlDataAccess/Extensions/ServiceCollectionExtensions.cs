using JobAppFlow.SqlDataAccess;
using JobAppFlow.SqlDataAccess.Data;
using JobAppFlow.SqlDataAccess.Models;
using JobAppFlow.SqlDataAccess.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobAppFlow.SqlDataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobAppFlowSqlDataAccess(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName = JobAppFlowSqlDataAccessConstants.ConnectionStringName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var connectionString = configuration.GetConnectionString(connectionStringName);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Missing connection string '{connectionStringName}'.");
        }

        services.AddDbContext<JobAppFlowDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.EnableRetryOnFailure(maxRetryCount: 2);
            });
        });

        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 0;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<JobAppFlowDbContext>();

        services.AddScoped<IUserAdministrationService, UserAdministrationService>();

        return services;
    }
}
