using JobAppFlow.Tools.IdentitySchemaGenerator;
using JobAppFlow.Tools.IdentitySchemaGenerator.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

var generationOptions = configuration
    .GetRequiredSection(IdentitySchemaGenerationOptions.SectionName)
    .Get<IdentitySchemaGenerationOptions>()
    ?? throw new InvalidOperationException("Missing identity schema generation options.");

var outputPath = ResolveOutputPath(args, generationOptions);

// EF Core uses the SQL Server provider to infer the exact Identity DDL shape.
// The generator does not connect to a real database and never issues CREATE DATABASE
// or any other data-changing command. Because of that, the connection string only
// needs to be syntactically valid enough for the provider configuration step.
// A fake LocalDB connection string keeps the tool self-contained and avoids
// introducing environment-specific settings into the repository.
const string fakeConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=_;Trusted_Connection=True;TrustServerCertificate=True";

var options = new DbContextOptionsBuilder<JobAppFlowIdentityDbContext>()
    .UseSqlServer(fakeConnectionString)
    .Options;

using var db = new JobAppFlowIdentityDbContext(options);
var sql = db.Database.GenerateCreateScript();

Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
File.WriteAllText(outputPath, sql);

Console.WriteLine($"Identity schema generated: {outputPath}");

static string ResolveOutputPath(string[] args, IdentitySchemaGenerationOptions options)
{
    if (args.Length >= 2 && args[0].Equals("--output", StringComparison.OrdinalIgnoreCase))
    {
        var supplied = args[1];
        return Path.IsPathRooted(supplied)
            ? supplied
            : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, supplied));
    }

    return Path.GetFullPath(Path.Combine(GetRepositoryRootDirectory(options.BaseDirectoryDepth), options.DefaultOutputPath));
}

static string GetRepositoryRootDirectory(int baseDirectoryDepth)
{
    var repoRoot = AppContext.BaseDirectory;
    for (var i = 0; i < baseDirectoryDepth; i++)
    {
        repoRoot = Path.GetFullPath(Path.Combine(repoRoot, ".."));
    }

    return repoRoot;
}
