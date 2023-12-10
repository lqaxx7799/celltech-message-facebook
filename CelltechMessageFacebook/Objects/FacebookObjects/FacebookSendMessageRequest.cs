using Newtonsoft.Json;

namespace CelltechMessageFacebook.Objects.FacebookObjects;

public class FacebookSendMessageRequest
{
    public FacebookSendMessageRecipientRequest Recipient { get; set; } = default!;
    [JsonProperty("message_typw")]
    public string MessageType { get; set; } = default!;

    public FacebookSendMessageMessageRequest Message { get; set; } = default!;
}

public class FacebookSendMessageRecipientRequest
{
    public string Id { get; set; } = default!;
}

public class FacebookSendMessageMessageRequest
{
    public string Text { get; set; } = default!;
}