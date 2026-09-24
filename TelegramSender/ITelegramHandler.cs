namespace Telegram;

public interface ITelegramHandler
{
    public Task SendMessageAsync(string message, int? chatId = null, CancellationToken ct = default);
    public Task RunTelegramBotAsync(CancellationToken ct = default);

}