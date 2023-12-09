using Newtonsoft.Json;

namespace CelltechMessageFacebook.Objects.FacebookObjects;

public class FacebookPageResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = default!;
    public string Category { get; set; } = default!;
    [JsonProperty("category_list")]
    public List<FacebookPageCategory> CategoryList { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Id { get; set; } = default!;
    public List<string> Tasks { get; set; } = default!;
}

public class FacebookPageCategory
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}