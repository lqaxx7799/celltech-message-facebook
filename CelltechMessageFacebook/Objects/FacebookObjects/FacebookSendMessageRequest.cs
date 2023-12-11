using Newtonsoft.Json;

namespace CelltechMessageFacebook.Objects.FacebookObjects;

public class FacebookSendMessageRequest
{
    [JsonProperty("recipient")]
    public FacebookSendMessageRecipientRequest Recipient { get; set; } = default!;
    [JsonProperty("message_type")]
    public string MessageType { get; set; } = default!;
    [JsonProperty("message")]
    public FacebookSendMessageMessageRequest Message { get; set; } = default!;
}

public class FacebookSendMessageRecipientRequest
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;
}

public class FacebookSendMessageMessageRequest
{
    [JsonProperty("text")]
    public string Text { get; set; } = default!;
}