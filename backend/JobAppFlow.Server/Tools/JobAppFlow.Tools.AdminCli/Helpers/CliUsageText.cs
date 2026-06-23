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
  - The current build only parses commands and dispatches handlers.
  - Database ORM wiring will be added in a later branch.
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
