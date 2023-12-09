namespace CelltechMessageFacebook.Objects.RequestObjects;

public class SignUpRequest
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FacebookUserId { get; set; } = default!;
    public string UserProfilePictureUrl { get; set; } = default!;
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public List<SignUpPageRequest> Pages { get; set; } = default!;
}

public class SignUpPageRequest
{
    public string PageName { get; set; } = default!;
    public string PageId { get; set; } = default!;
    public string PageProfilePictureUrl { get; set; } = default!;
}