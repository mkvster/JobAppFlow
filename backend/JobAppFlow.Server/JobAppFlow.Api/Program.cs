using JobAppFlow.Api.Installers;
using JobAppFlow.SqlDataAccess.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

builder.Services.AddJobAppFlowSqlDataAccess(builder.Configuration);

var features = new FeatureInstallerCollection(
    new CorsInstaller(),
    new JwtAuthInstaller(),
    new SwaggerInstaller(),
    new ApplicationInsightsInstaller()
);

features.ConfigureBuilder(builder);

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHttpsRedirection();
    app.UseHsts();
}
app.UseStatusCodePages();

features.ConfigureApp(app);

app.MapControllers();
app.MapGet("/health", () => Results.Text("Healthy"));

app.Run();
