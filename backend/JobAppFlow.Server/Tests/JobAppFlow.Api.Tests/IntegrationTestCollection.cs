namespace JobAppFlow.Api.Tests;

public static class IntegrationTestCollectionDefinition
{
    public const string Name = "Integration tests";
}

[CollectionDefinition(IntegrationTestCollectionDefinition.Name)]
public sealed class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
{
}
