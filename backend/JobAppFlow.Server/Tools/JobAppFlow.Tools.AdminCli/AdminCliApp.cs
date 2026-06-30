using JobAppFlow.Tools.AdminCli.Helpers;
using JobAppFlow.Tools.AdminCli.Models;
using JobAppFlow.Tools.AdminCli.Services;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace JobAppFlow.Tools.AdminCli;

public sealed class AdminCliApp
{
    private readonly IAdminDbCommandProcessor _processor;

    public AdminCliApp(IAdminDbCommandProcessor processor)
    {
        _processor = processor;
    }

    public async Task<int> RunAsync(string[] args)
    {
        var normalizedArgs = CliArgumentNormalizer.Normalize(args);

        if (normalizedArgs.Length == 0 || normalizedArgs[0].Equals("help", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine(CliUsageText.Root());
            return 0;
        }

        var rootCommand = BuildRootCommand();
        var parseResult = rootCommand.Parse(normalizedArgs, new ParserConfiguration());
        return await parseResult.InvokeAsync(new InvocationConfiguration());
    }

    private RootCommand BuildRootCommand()
    {
        var rootCommand = new RootCommand("JobAppFlow admin CLI");

        var helpCommand = new Command("help", "Show general help.");
        helpCommand.SetAction(_ =>
        {
            Console.WriteLine(CliUsageText.Root());
            return 0;
        });
        rootCommand.Add(helpCommand);

        var dbCommand = new Command("db", "Database administration commands.");
        dbCommand.Aliases.Add("database");
        dbCommand.SetAction(_ =>
        {
            Console.WriteLine(CliUsageText.Database());
            return 0;
        });
        rootCommand.Add(dbCommand);

        var dbHelpCommand = new Command("help", "Show database help.");
        dbHelpCommand.SetAction(_ =>
        {
            Console.WriteLine(CliUsageText.Database());
            return 0;
        });
        dbCommand.Add(dbHelpCommand);

        dbCommand.Add(BuildAddUserCommand());
        dbCommand.Add(BuildRemoveUserCommand());
        dbCommand.Add(BuildBanUserCommand());
        dbCommand.Add(BuildUnbanUserCommand());
        dbCommand.Add(BuildAddRoleCommand());
        dbCommand.Add(BuildRemoveRoleCommand());
        dbCommand.Add(BuildAddUserRoleCommand());
        dbCommand.Add(BuildRemoveUserRoleCommand());

        return rootCommand;
    }

    private Command BuildAddUserCommand()
    {
        var command = new Command("add-user", "Create a new identity user.");
        var loginOption = new Option<string>("--login", Array.Empty<string>())
        {
            Description = "User login.",
            Required = true,
        };
        var passwordOption = new Option<string>("--pwd", Array.Empty<string>())
        {
            Description = "User password.",
            Required = true,
        };
        var emailOption = new Option<string>("--email", Array.Empty<string>())
        {
            Description = "User e-mail address.",
            Required = true,
        };

        command.Add(loginOption);
        command.Add(passwordOption);
        command.Add(emailOption);
        command.SetAction(async parseResult =>
        {
            return await ExecuteCommandAsync(async () =>
            {
                var login = parseResult.GetRequiredValue(loginOption);
                var password = parseResult.GetRequiredValue(passwordOption);
                var email = parseResult.GetRequiredValue(emailOption);
                await _processor.AddUserAsync(new AddUserRequest(login, password, email));
            });
        });

        return command;
    }

    private Command BuildRemoveUserCommand()
    {
        var command = new Command("remove-user", "Remove an identity user.");
        var loginOption = new Option<string>("--login", Array.Empty<string>())
        {
            Description = "User login.",
            Required = true,
        };

        command.Add(loginOption);
        command.SetAction(async parseResult =>
        {
            return await ExecuteCommandAsync(async () =>
            {
                var login = parseResult.GetRequiredValue(loginOption);
                await _processor.RemoveUserAsync(new RemoveUserRequest(login));
            });
        });

        return command;
    }

    private Command BuildBanUserCommand()
    {
        var command = new Command("ban-user", "Disable an identity user.");
        var loginOption = new Option<string>("--login", Array.Empty<string>())
        {
            Description = "User login.",
            Required = true,
        };

        command.Add(loginOption);
        command.SetAction(async parseResult =>
        {
            return await ExecuteCommandAsync(async () =>
            {
                var login = parseResult.GetRequiredValue(loginOption);
                await _processor.BanUserAsync(new BanUserRequest(login));
            });
        });

        return command;
    }

    private Command BuildUnbanUserCommand()
    {
        var command = new Command("unban-user", "Re-enable an identity user.");
        var loginOption = new Option<string>("--login", Array.Empty<string>())
        {
            Description = "User login.",
            Required = true,
        };

        command.Add(loginOption);
        command.SetAction(async parseResult =>
        {
            return await ExecuteCommandAsync(async () =>
            {
                var login = parseResult.GetRequiredValue(loginOption);
                await _processor.UnbanUserAsync(new UnbanUserRequest(login));
            });
        });

        return command;
    }

    private Command BuildAddRoleCommand()
    {
        var command = new Command("add-role", "Create a new identity role.");
        var roleArgument = new Argument<string>("role_name")
        {
            Description = "Role name.",
        };

        command.Add(roleArgument);
        command.SetAction(async parseResult =>
        {
            return await ExecuteCommandAsync(async () =>
            {
                var roleName = parseResult.GetRequiredValue(roleArgument);
                await _processor.AddRoleAsync(new AddRoleRequest(roleName));
            });
        });

        return command;
    }

    private Command BuildRemoveRoleCommand()
    {
        var command = new Command("remove-role", "Remove an identity role.");
        var roleArgument = new Argument<string>("role_name")
        {
            Description = "Role name.",
        };

        command.Add(roleArgument);
        command.SetAction(async parseResult =>
        {
            return await ExecuteCommandAsync(async () =>
            {
                var roleName = parseResult.GetRequiredValue(roleArgument);
                await _processor.RemoveRoleAsync(new RemoveRoleRequest(roleName));
            });
        });

        return command;
    }

    private Command BuildAddUserRoleCommand()
    {
        var command = new Command("add-user-role", "Assign a role to an identity user.");
        var loginOption = new Option<string>("--login", Array.Empty<string>())
        {
            Description = "User login.",
            Required = true,
        };
        var roleOption = new Option<string>("--role", Array.Empty<string>())
        {
            Description = "Role name.",
            Required = true,
        };

        command.Add(loginOption);
        command.Add(roleOption);
        command.SetAction(async parseResult =>
        {
            return await ExecuteCommandAsync(async () =>
            {
                var login = parseResult.GetRequiredValue(loginOption);
                var roleName = parseResult.GetRequiredValue(roleOption);
                await _processor.AddUserRoleAsync(new AddUserRoleRequest(login, roleName));
            });
        });

        return command;
    }

    private Command BuildRemoveUserRoleCommand()
    {
        var command = new Command("remove-user-role", "Remove a role from an identity user.");
        var loginOption = new Option<string>("--login", Array.Empty<string>())
        {
            Description = "User login.",
            Required = true,
        };
        var roleOption = new Option<string>("--role", Array.Empty<string>())
        {
            Description = "Role name.",
            Required = true,
        };

        command.Add(loginOption);
        command.Add(roleOption);
        command.SetAction(async parseResult =>
        {
            return await ExecuteCommandAsync(async () =>
            {
                var login = parseResult.GetRequiredValue(loginOption);
                var roleName = parseResult.GetRequiredValue(roleOption);
                await _processor.RemoveUserRoleAsync(new RemoveUserRoleRequest(login, roleName));
            });
        });

        return command;
    }

    private static async Task<int> ExecuteCommandAsync(Func<Task> action)
    {
        try
        {
            await action();
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            throw;
        }
    }
}
