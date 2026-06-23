namespace JobAppFlow.Tools.IdentitySchemaGenerator;

public sealed class IdentitySchemaGenerationOptions
{
    public const string SectionName = "IdentitySchemaGeneration";

    public string DefaultOutputPath { get; init; } = string.Empty;
    public int BaseDirectoryDepth { get; init; }
}
