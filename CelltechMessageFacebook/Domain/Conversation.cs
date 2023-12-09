namespace CelltechMessageFacebook.Domain;

public class Conversation : BaseEntity
{
    public List<Guid> UserIds { get; set; } = default!;
    public Guid CustomerId { get; set; }
    public Guid FacebookPageId { get; set; }
}