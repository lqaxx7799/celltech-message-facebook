namespace CelltechMessageFacebook.Domain;

public class MessageBlock
{
    public Guid Id { get; set; }
    public string Content { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public Guid MessageId { get; set; }
}