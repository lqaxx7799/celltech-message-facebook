namespace CelltechMessageFacebook.Domain;

public class Conversation
{
    public Guid Id { get; set; }
    public List<Guid> UserIds { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CustomerId { get; set; }
    public Guid FacebookPageId { get; set; }
}