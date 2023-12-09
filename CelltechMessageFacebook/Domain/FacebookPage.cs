namespace CelltechMessageFacebook.Domain;

public class FacebookPage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public Guid UserId { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public string PageId { get; set; } = default!;
}