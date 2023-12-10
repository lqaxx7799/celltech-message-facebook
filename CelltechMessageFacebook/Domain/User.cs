namespace CelltechMessageFacebook.Domain;

public class User : BaseEntity
{
    public string UserName { get; set; } = default!;
    public string? Email { get; set; }
    public UserType Type { get; set; }
    public string? FacebookAccountId { get; set; }
    public string? AccountAccessToken { get; set; }
    public bool IsFacebookConnected { get; set; }
    public Guid? UserOwnerId { get; set; }
}

public enum UserType
{
    Agent = 1,
    Customer
}