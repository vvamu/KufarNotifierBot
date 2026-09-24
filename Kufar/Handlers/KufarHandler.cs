using Kufar.Handlers;
using Kufar.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Kufar;

public partial class KufarHandler : IKufarHandler
{
    private readonly HttpClient _httpClient;
    private ILogger? _logger;
    public KufarHandler(HttpClient client, ILogger logger = null)
    {
        _httpClient = client;
        _logger = logger;
    }
    public async Task<IEnumerable<Conversation>> GetMessagesAsync(int countMessages, CancellationToken ct = default)
    {
        if (countMessages <= 0) throw new ArgumentOutOfRangeException(nameof(countMessages));
        try
        {
            var messages = await FetchMessagesAsync(countMessages, ct);
            return messages;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("Kufar API network error: " + ex.ToString());
        }
        catch (JsonException ex)
        {
            _logger.LogError("Kufar API returned unexpected JSON: " + ex.ToString());
        }
        return new List<Conversation>();
    }
    public async Task<IEnumerable<Conversation>> GetNewMessagesAsync(CancellationToken ct = default)
    {
        try
        {
            var countMessages = await FetchCountChatsWithUnreadAsync(ct);
            if(countMessages == 0) return new List<Conversation>();
            var messages = await GetMessagesAsync(countMessages);
            return messages ?? new List<Conversation>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("Kufar API network error: " + ex.ToString());
        }
        catch (JsonException ex)
        {
            _logger.LogError("Kufar API returned unexpected JSON: " + ex.ToString());
        }
        return new List<Conversation>();

    }
    private async Task<int> FetchCountChatsWithUnreadAsync(CancellationToken ct)
    {

        var resJsonString = await _httpClient.GetStringAsync(KufarEndpoints.V1Counter);
        using var doc = JsonDocument.Parse(resJsonString);
        var countUnreadMessages = doc.RootElement.GetProperty("unread").GetInt16();
        if(countUnreadMessages == 0) return 0;

        var countChats = await CountChatsWithUnreadAsync(countUnreadMessages, ct);
        return countChats;

    }

    private async Task<int> CountChatsWithUnreadAsync(int countUnreadMessages, CancellationToken ct)
    {
        const int pageSize = 5;
        var offset = 0;
        var counterMessages = 0;
        var countChats = 0;

        while (counterMessages < countUnreadMessages)
        {
            var page = await FetchMessagesAsync(pageSize, ct);
            if (page.Count() == 0)
                break;

            foreach (var item in page)
            {
                counterMessages += (int)item.Unseen;
                countChats++;

                if (counterMessages >= countUnreadMessages)
                    break;
            }

            offset += pageSize;
        }

        return countChats;
    }

    private async Task<IEnumerable<Conversation>> FetchMessagesAsync(int countMessages, CancellationToken ct)
    {

        var resJsonString = await _httpClient.GetStringAsync(KufarEndpoints.V4ConversationsWithPaging(countMessages,0));
        using var doc = JsonDocument.Parse(resJsonString);

        List<Conversation> conversationList = new List<Conversation>();

        foreach (var conv in doc.RootElement.GetProperty("conversations").EnumerateArray())
        {
            var element = new Conversation(
            
                Subject : conv.GetProperty("ad_info").GetProperty("subject").GetString(),
                ParticipantName: conv.GetProperty("participant_info").GetProperty("name").GetString(),
                LastMessagePreview: conv.GetProperty("last_message").GetProperty("preview").GetString(),
                LastMessageTimestamp: DateTimeOffset.Parse(conv.GetProperty("last_message").GetProperty("timestamp").GetString(), CultureInfo.InvariantCulture)
                    .ToLocalTime().ToString("d MMMM, HH:mm", new CultureInfo("ru-RU")),
                Unseen: conv.GetProperty("unseen").GetInt32()
            );
            conversationList.Add(element);
        }

        return conversationList;
    }

}
