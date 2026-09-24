using Kufar.Handlers;
using Kufar.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Telegram;
using WpfApplication.Helpers;

namespace WpfApplication.ViewModels;

public class MainWindowViewModel : ViewModel
{
    private readonly IKufarHandler _kufarHandler;
    private readonly ITelegramHandler _telegramSender;
    private List<Conversation> _conversations = new();
    public List<Conversation> Conversations
    {
        get => _conversations;
        private set
        {
            _conversations = value;
            OnPropertyChanged(); // ← одно уведомление: «свойство изменилось»
        }
    }
    public ICommand KufarUpdateMessagesCommand { get; }
    public ICommand SendTelegramMessagesCommand { get; }

    public MainWindowViewModel(IKufarHandler kufarHandler, ITelegramHandler telegramSender)
    {
        _kufarHandler = kufarHandler;
        _telegramSender = telegramSender;
        KufarUpdateMessagesCommand = new LambdaCommand(KufarUpdateMessages, true);
        SendTelegramMessagesCommand = new LambdaCommand(SendTelegramMessages, true);

    }

    private async Task KufarUpdateMessages()
    {
        var messagesKufar = await _kufarHandler?.GetMessagesAsync(10) ?? new ObservableCollection<Conversation>();
        Conversations = messagesKufar.ToList();

    }

    private async Task SendTelegramMessages()
    {
        var message = "hello";
        await _telegramSender.SendMessageAsync(message);
    }

}