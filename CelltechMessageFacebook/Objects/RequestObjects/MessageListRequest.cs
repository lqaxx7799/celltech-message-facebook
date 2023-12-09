namespace CelltechMessageFacebook.Objects.RequestObjects;

public class MessageListRequest
{
    public Guid ConversationId { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}