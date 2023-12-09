using CelltechMessageFacebook.Objects.FacebookObjects;

namespace CelltechMessageFacebook.Objects.RequestObjects;

public class SignUpRequest
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public FacebookAuthResponse AuthResponse { get; set; } = default!;
    public List<SignUpPageRequest> Pages { get; set; } = default!;
}

public class SignUpPageRequest
{
    public string PageName { get; set; } = default!;
    public string PageId { get; set; } = default!;
    public string? PageProfilePictureUrl { get; set; }
    public string AccessToken { get; set; } = default!;
}