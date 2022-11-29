using System.ComponentModel.DataAnnotations;

namespace Challenge.Infrastructure.Data.Models;

public class ChatMessage
{
    [Key] public int ChatMessageId { get; set; }
    public string User { get; set; }
    public string Message { get; set; }
    public DateTimeOffset ReceivedDateTime { get; set; } = DateTime.UtcNow;
}