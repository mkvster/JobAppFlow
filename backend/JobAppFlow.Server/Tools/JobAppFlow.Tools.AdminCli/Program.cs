using JobAppFlow.SqlDataAccess.Extensions;
using JobAppFlow.Tools.AdminCli;
using JobAppFlow.Tools.AdminCli.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.Local.json", optional: true)
    .Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddJobAppFlowSqlDataAccess(configuration);
services.AddScoped<IAdminDbCommandProcessor, AdminDbCommandProcessor>();

using var serviceProvider = services.BuildServiceProvider();
using var scope = serviceProvider.CreateScope();

var app = new AdminCliApp(scope.ServiceProvider.GetRequiredService<IAdminDbCommandProcessor>());
return await app.RunAsync(args);
