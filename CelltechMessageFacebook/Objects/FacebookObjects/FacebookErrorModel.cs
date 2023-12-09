using Newtonsoft.Json;

namespace CelltechMessageFacebook.Objects.FacebookObjects;

public class FacebookErrorModel
{
    public FacebookErrorData Error { get; set; } = default!;
}

public class FacebookErrorData
{
    public string? Message { get; set; }
    
    public string? Type { get; set; }
    
    public string? Code { get; set; }
    
    [JsonProperty("error_subcode")]
    public string? ErrorSubCode { get; set; }
    
    [JsonProperty("fbtrace_id")]
    public  string? FbTraceId { get; set; }
}