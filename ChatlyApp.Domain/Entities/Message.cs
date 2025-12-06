namespace ChatlyApp.Domain.Entities;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string SenderId { get; set; } = string.Empty;
    public User? Sender { get; set; }

    public string ReceiverId { get; set; } = string.Empty;
    public User? Receiver { get; set; }
}
