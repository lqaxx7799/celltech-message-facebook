using Newtonsoft.Json;

namespace CelltechMessageFacebook.Objects.FacebookObjects;

public class FacebookHookEntry
{
    public string Id { get; set; } = default!;
    public long Time { get; set; }
    public List<FacebookHookChange> Changes { get; set; } = default!;
}

public class FacebookHookChange
{
    public FacebookHookValue Value { get; set; } = default!;
    public string Field { get; set; } = default!;
}

public class FacebookHookValue
{
    [JsonProperty("ad_id")]
    public string? AdId { get; set; }
    
    [JsonProperty("form_id")]
    public string? FormId { get; set; }
    
    [JsonProperty("leadgen_id")]
    public string? LeadgenId { get; set; }
    
    [JsonProperty("created_time")]
    public long CreatedTime { get; set; }
    
    [JsonProperty("page_id")]
    public string? PageId { get; set; }
    
    [JsonProperty("adgroup_id")]
    public string? AdgroupId { get; set; }
}