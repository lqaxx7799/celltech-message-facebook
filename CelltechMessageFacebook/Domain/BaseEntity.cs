namespace CelltechMessageFacebook.Domain;

public class BaseEntity
{
    public Guid Id { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid ModifiedBy { get; set; }
}