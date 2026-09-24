using ConsoleApplication.Commands.Base;
using Kufar.Handlers;
using Kufar.Models;
using Microsoft.Extensions.Logging;
using Telegram;

namespace ConsoleApplication.Commands.Kufar;

public class RunTelegramBotCommand : ICommand
{
    private readonly ITelegramHandler _telegram;
    private readonly ILogger<SyncCommand> _logger;

    public RunTelegramBotCommand(IKufarHandler kufar,ITelegramHandler telegram,ILogger<SyncCommand> logger)
    {
        _telegram = telegram;
        _logger = logger;
    }

    public string Name => "telegram-handler";
    public string[] Aliases => new[] { "telegram", "get-messeges-telegram" };
    public string Description => "Run Telegram bot to handle messeges";

    public async Task<int> ExecuteAsync(string[] args, CancellationToken ct = default)
    {
        _logger.LogInformation("Starting Run TelegramBot");
        await _telegram.RunTelegramBotAsync();
        Console.ReadLine();
        return 1;
       
    }

}