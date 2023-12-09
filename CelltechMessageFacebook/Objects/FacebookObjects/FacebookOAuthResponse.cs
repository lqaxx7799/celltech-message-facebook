using Newtonsoft.Json;

namespace CelltechMessageFacebook.Objects.FacebookObjects;

public class FacebookOAuthResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = default!;
    [JsonProperty("token_type")]
    public string TokenType { get; set; } = default!;
    [JsonProperty("expires_in")]
    public long ExpiresIn { get; set; }
}