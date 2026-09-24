using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using Telegram;

namespace Main.Application;

public class TelegramAPI
{
    private readonly ITelegramHandler _telegramHandler;

    public TelegramAPI(ITelegramHandler telegramHandler)
    {
        _telegramHandler = telegramHandler;
    }
    public async Task SendTelegramMessages()
    {
        var message = "hello";
        await _telegramHandler.Run(message);
    }

}
