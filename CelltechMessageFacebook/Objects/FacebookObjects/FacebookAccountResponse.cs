using Newtonsoft.Json;

namespace CelltechMessageFacebook.Objects.FacebookObjects;

public class FacebookAccountResponse
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    [JsonProperty("first_name")]
    public string? FirstName { get; set; }
    [JsonProperty("last_name")]
    public string? LastName { get; set; }
}

