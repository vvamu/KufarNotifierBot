using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication.Commands.Base;

public class CommandRunner
{
    private readonly Dictionary<string, ICommand> _commandMap;
    private readonly IEnumerable<ICommand> _allCommands;

    public CommandRunner(IEnumerable<ICommand> commands)
    {
        _allCommands = commands;
        _commandMap = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase);

        foreach (var cmd in commands)
        {
            _commandMap[cmd.Name] = cmd;
            foreach (var alias in cmd.Aliases)
                _commandMap[alias] = cmd;
        }
    }

    public async Task<int> RunAsync(string[] args, CancellationToken ct = default)
    {
        if (args.Length == 0)
        {
            ShowHelp();
            return 0;
        }

        if (!_commandMap.TryGetValue(args[0], out var command))
        {
            Console.WriteLine($"Unknown command: '{args[0]}'");
            ShowHelp();
            return 1;
        }

        if (command is HelpCommand)
        {
            ShowHelp();
            return 0;
        }

        try
        {
            return await command.ExecuteAsync(args.Skip(1).ToArray(),ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }

    private void ShowHelp()
    {
        Console.WriteLine("Usage:");
        foreach (var cmd in _allCommands.Where(c => c is not HelpCommand))
        {
            var aliases = cmd.Aliases.Length > 0
                ? $" ({string.Join(", ", cmd.Aliases)})"
                : "";
            Console.WriteLine($"  app {cmd.Name}{aliases} - {cmd.Description}");
        }
    }
}