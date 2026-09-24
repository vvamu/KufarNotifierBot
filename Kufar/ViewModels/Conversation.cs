namespace Kufar.Models;
public record Conversation(string? Subject, 
    string? ParticipantName, 
    string? LastMessagePreview,
    string? LastMessageTimestamp,
    int? Unseen)
{
    public override string ToString() =>
    $"""
        📩 Новое сообщение на Kufar

        Тема: {Subject}
        От: {ParticipantName}
        Превью: {LastMessagePreview}
        Время: {LastMessageTimestamp}
        """;
    
}

