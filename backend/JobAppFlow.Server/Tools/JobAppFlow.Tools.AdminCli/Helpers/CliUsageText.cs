namespace JobAppFlow.Tools.AdminCli.Helpers;

internal static class CliUsageText
{
    public static string Root() => """
Usage:
  jaf help
  jaf db help
  jaf db -add-user -login <login> -pwd <password> -email <email>
  jaf db -remove-user -login <login>
  jaf db -ban-user -login <login>
  jaf db -unban-user -login <login>

Notes:
  - Commands are executed against the configured SQL Server identity store.
  - Connection string: ConnectionStrings:JobAppFlowDb.
""";

    public static string Database() => """
Usage:
  jaf db help
  jaf db -add-user -login <login> -pwd <password> -email <email>
  jaf db -remove-user -login <login>
  jaf db -ban-user -login <login>
  jaf db -unban-user -login <login>
""";
}
