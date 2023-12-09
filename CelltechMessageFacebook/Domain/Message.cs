namespace CelltechMessageFacebook.Domain;

public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid ConversationId { get; set; }
    
}