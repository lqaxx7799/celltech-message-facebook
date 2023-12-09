namespace CelltechMessageFacebook.Domain;

public class Message : BaseEntity
{
    public Guid SenderId { get; set; }
    public Guid ConversationId { get; set; }
}