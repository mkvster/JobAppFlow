namespace JobAppFlow.Tools.IdentitySchemaGenerator;

public sealed class OutputOptions
{
    public const string SectionName = "Output";

    public string DefaultOutputPath { get; init; } = string.Empty;
    public int BaseDirectoryDepth { get; init; }
}
