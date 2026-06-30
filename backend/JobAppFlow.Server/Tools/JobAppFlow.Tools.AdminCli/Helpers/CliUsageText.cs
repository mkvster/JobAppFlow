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
  jaf db -add-role <role_name>
  jaf db -remove-role <role_name>
  jaf db -add-user-role -login <login> -role <role_name>
  jaf db -remove-user-role -login <login> -role <role_name>

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
  jaf db -add-role <role_name>
  jaf db -remove-role <role_name>
  jaf db -add-user-role -login <login> -role <role_name>
  jaf db -remove-user-role -login <login> -role <role_name>
""";
}
