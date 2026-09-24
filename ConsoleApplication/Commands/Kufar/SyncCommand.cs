using ConsoleApplication.Commands.Base;
using Kufar.Handlers;
using Kufar.Models;
using Microsoft.Extensions.Logging;
using Telegram;

namespace ConsoleApplication.Commands.Kufar;

public class SyncCommand : ICommand
{
    private readonly IKufarHandler _kufar;
    private readonly ITelegramHandler _telegram;
    private readonly ILogger<SyncCommand> _logger;

    public SyncCommand(IKufarHandler kufar,ITelegramHandler telegram,ILogger<SyncCommand> logger)
    {
        _kufar = kufar;
        _telegram = telegram;
        _logger = logger;
    }

    public string Name => "sync";
    public string[] Aliases => new[] { "update", "check" };
    public string Description => "Fetch new Kufar messages and send them to Telegram";

    public async Task<int> ExecuteAsync(string[] args, CancellationToken ct = default)
    {
        _logger.LogInformation("Starting Kufar - Telegram sync");

        try
        {
            // 1. Получаем новые сообщения
            var messages = await _kufar.GetNewMessagesAsync(ct);


            _logger.LogInformation("Found {Count} new messages", messages.Count());
            // 2. Отправляем каждое в Telegram
            foreach (var msg in messages)
            {
                var text = msg.ToString();
                await _telegram.SendMessageAsync(text,ct:ct);
                Console.WriteLine($"Sent: {msg.Subject}");
            }

            _logger.LogInformation("Sync completed: {Count} messages sent", messages.Count());
            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sync failed");
            Console.WriteLine($"Error: {ex.Message}");
            
            return 1;
        }
       
    }


}