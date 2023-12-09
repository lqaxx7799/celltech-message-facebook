namespace CelltechMessageFacebook.Objects;

public class MessageReplyRequest
{
    public string Content { get; set; } = default!;
    public Guid SenderId { get; set; }
    public Guid ConversationId { get; set; }
}