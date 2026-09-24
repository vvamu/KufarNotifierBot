using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace Main.Application;

public class KufarAPI
{
    public KufarAPI()
    {

    }
    private async Task KufarUpdateMessages()
    {
        var messagesKufar = await _kufarHandler?.GetMessagesAsync(10) ?? new ObservableCollection<Conversation>();
        Conversations = messagesKufar.ToList();

    }

    private async Task SendTelegramMessages()
    {
        var message = "hello";
        await _telegramSender.Run(message);
    }

}
