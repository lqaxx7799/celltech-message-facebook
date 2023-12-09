namespace CelltechMessageFacebook.Domain;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string? Email { get; set; }
    public UserType Type { get; set; }
}

public enum UserType
{
    Agent = 1,
    Customer
}