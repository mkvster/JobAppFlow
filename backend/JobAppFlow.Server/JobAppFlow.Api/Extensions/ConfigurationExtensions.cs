namespace JobAppFlow.Api.Extensions;

public static class ConfigurationExtensions
{
    public static T ReadSection<T>(this IConfiguration configuration, string sectionName)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return configuration.GetRequiredSection(sectionName).Get<T>()
            ?? throw new InvalidOperationException($"{sectionName} configuration section is required.");
    }
}
