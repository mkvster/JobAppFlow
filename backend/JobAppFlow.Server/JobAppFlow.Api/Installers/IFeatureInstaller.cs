namespace JobAppFlow.Api.Installers;

public interface IFeatureInstaller {
    void ConfigureBuilder(IHostApplicationBuilder builder);
    void ConfigureApp(IApplicationBuilder app);
}

public sealed class FeatureInstallerCollection {
    private readonly IReadOnlyList<IFeatureInstaller> _installers;

    public FeatureInstallerCollection(params IFeatureInstaller[] installers) {
        _installers = installers;
    }

    public void ConfigureBuilder(WebApplicationBuilder builder) {
        foreach (var installer in _installers) {
            installer.ConfigureBuilder(builder);
        }
    }

    public void ConfigureApp(WebApplication app) {
        foreach (var installer in _installers) {
            installer.ConfigureApp(app);
        }
    }
}
