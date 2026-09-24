using System.ComponentModel.DataAnnotations;

namespace Telegram.Options;

public record TelegramOption{
    [Required]
    public string botToken { get; init; }
    [Required]
    public int? adminChatId { get; init; }
};
