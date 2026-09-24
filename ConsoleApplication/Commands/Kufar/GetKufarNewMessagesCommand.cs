using ConsoleApplication.Commands.Base;
using Kufar.Handlers;

namespace ConsoleApplication.Commands.Kufar;

public sealed class GetKufarNewMessagesCommand : ICommand
{
    private IKufarHandler _kufarHandler;
    public string Name => "get_kufar_new_messages";
    
    public string Description => "Fetch new Kufar messages";

    public string[] Aliases => ["kufar","new","messages"];

    public GetKufarNewMessagesCommand(IKufarHandler kufarHandler) { _kufarHandler = kufarHandler; }

    public async Task<int> ExecuteAsync(string[] args, CancellationToken ct = default)
    {
        var messages = await _kufarHandler.GetNewMessagesAsync();
        foreach (var msg in messages)
            Console.WriteLine($"[{msg.LastMessageTimestamp}] {msg.ParticipantName}: {msg.Subject}");
        return 0;
    }
}
