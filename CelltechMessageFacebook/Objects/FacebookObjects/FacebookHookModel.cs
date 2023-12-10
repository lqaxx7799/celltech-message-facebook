using Newtonsoft.Json;

namespace CelltechMessageFacebook.Objects.FacebookObjects;

public class FacebookHookEntry
{
    public string Id { get; set; } = default!;
    public long Time { get; set; }
    public List<FacebookHookMessaging> Messaging { get; set; } = default!;
}

public class FacebookHookMessaging
{
    public FacebookHookMessageUser Sender { get; set; } = default!;
    public FacebookHookMessageUser Recipient { get; set; } = default!;
    public long Timestamp { get; set; }
    public FacebookHookMessage Message { get; set; } = default!;
}

public class FacebookHookMessageUser
{
    public string Id { get; set; } = default!;
}

public class FacebookHookMessage
{
    public string Mid { get; set; } = default!;

    public string Text { get; set; } = default!;
}