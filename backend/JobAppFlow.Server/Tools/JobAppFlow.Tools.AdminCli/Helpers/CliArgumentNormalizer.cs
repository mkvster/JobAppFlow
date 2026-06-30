namespace JobAppFlow.Tools.AdminCli.Helpers;

internal static class CliArgumentNormalizer
{
    private static readonly Dictionary<string, string> CommandAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["-add-user"] = "add-user",
        ["-remove-user"] = "remove-user",
        ["-ban-user"] = "ban-user",
        ["-unban-user"] = "unban-user",
        ["-add-role"] = "add-role",
        ["-remove-role"] = "remove-role",
        ["-add-user-role"] = "add-user-role",
        ["-remove-user-role"] = "remove-user-role",
        ["-help"] = "help",
        ["database"] = "db",
    };

    private static readonly HashSet<string> OptionAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        "-login",
        "-pwd",
        "-email",
        "-role",
    };

    public static string[] Normalize(string[] args)
    {
        var normalized = new string[args.Length];

        for (var i = 0; i < args.Length; i++)
        {
            normalized[i] = NormalizeToken(args[i]);
        }

        return normalized;
    }

    private static string NormalizeToken(string token)
    {
        if (CommandAliases.TryGetValue(token, out var commandAlias))
        {
            return commandAlias;
        }

        if (OptionAliases.Contains(token))
        {
            return "-" + token;
        }

        return token;
    }
}
