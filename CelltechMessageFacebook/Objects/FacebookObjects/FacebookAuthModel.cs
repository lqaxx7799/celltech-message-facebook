using Newtonsoft.Json;

namespace CelltechMessageFacebook.Objects.FacebookObjects;

public class FacebookAuthModel
{
    public FacebookAuthResponse AuthResponse { get; set; } = default!;
    public string Status { get; set; } = default!;
}

public class FacebookAuthResponse
{
    public string AccessToken { get; set; } = default!;
    [JsonProperty("data_access_expiration_time")]
    public string DataAccessExpirationTime { get; set; } = default!;
    public long ExpiresIn { get; set; }
    public string GraphDomain { get; set; } = default!;
    public string SignedRequest { get; set; } = default!;
    [JsonProperty("userID")]
    public string UserId { get; set; } = default!;
}