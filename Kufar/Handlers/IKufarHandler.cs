using Kufar.Models;

namespace Kufar.Handlers;

public interface IKufarHandler
{
    public Task<IEnumerable<Conversation>> GetMessagesAsync(int countMessages, CancellationToken ct = default);
    public Task<IEnumerable<Conversation>> GetNewMessagesAsync(CancellationToken ct = default);

}