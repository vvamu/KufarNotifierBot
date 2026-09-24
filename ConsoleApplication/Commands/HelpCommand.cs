using ConsoleApplication.Commands.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication.Commands;

public class HelpCommand : ICommand
{
    public string Name => "help";
    public string[] Aliases => new[] { "?", "-h", "--help" };
    public string Description => "Show help";

    // Никаких зависимостей!
    public Task<int> ExecuteAsync(string[] args, CancellationToken ct = default)
    {
        // Справку выводит CommandRunner, HelpCommand просто сигнализирует
        return Task.FromResult(0);
    }
}
