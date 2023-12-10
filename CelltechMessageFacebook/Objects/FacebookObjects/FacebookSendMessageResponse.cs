using Newtonsoft.Json;

namespace CelltechMessageFacebook.Objects.FacebookObjects;

public class FacebookSendMessageResponse
{
    [JsonProperty("recipient_id")]
    public string RecipientId { get; set; } = default!;

    [JsonProperty("message_id")]
    public string MessageId { get; set; } = default!;
}