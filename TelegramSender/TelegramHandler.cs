using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Options;
using static Telegram.Bot.TelegramBotClient;

namespace Telegram;

public class TelegramHandler : ITelegramHandler
{    
    private static ITelegramBotClient _botClient;
    private static ReceiverOptions _receiverOptions;
    private readonly ILogger? _logger;
    private TelegramOption _config;


    public TelegramHandler(IOptions<TelegramOption> options, ILogger? logger = null)
    {
        _config = options.Value;

        if (string.IsNullOrWhiteSpace(_config.botToken)) throw new ArgumentNullException(nameof(_config.botToken));
        _botClient = new TelegramBotClient(token: _config.botToken);
        _logger = logger;
        _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[]
            {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
            },

        };
    }
    public async Task SendMessageAsync(string message, int? chatId = null, CancellationToken ct = default)
    {
        try
        {
            if (_botClient == null) throw new Exception("Telegram bot client is null");
            if(_config == null) throw new Exception("Configuration not set");
            if (_config?.adminChatId == null) throw new Exception("Configuration not set parameter to admin chat id");

            await SendMessage(chatId ?? (int)_config.adminChatId, message, ct);
        }
        catch (Exception ex)
        {
            _logger.LogError("Message to telegram not sended: " + ex.Message);
        }
        
    }


    public async Task RunTelegramBotAsync(CancellationToken ct = default)
    {
        try
        {
            if (_botClient == null) throw new Exception("Telegram bot client is null");

            _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, ct);
            await Task.Delay(-1); 
        }
        catch (Exception ex)
        {
            _logger.LogError("Message to telegram not sended: " + ex.Message);
        }
    }

    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        Console.WriteLine("Пришло сообщение!");
                        return;
                    }
            }
        }
        catch (Exception ex) { throw new Exception("Exeption in method ITelegramBotClient UpdateHandler(...): " + ex.Message); }

    }

    private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }


    private async Task SendMessage(int chatId ,string message, CancellationToken cancellationToken)
    {
        try
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: string.IsNullOrEmpty(message) ? $"Hey! I recognized your message 🎯: " : message,
                cancellationToken:cancellationToken
            );
        }
        catch (Exception ex) { throw new Exception("Exeption in method ITelegramBotClient SendMessage(...): " + ex.Message); }

    }
}
