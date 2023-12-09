namespace CelltechMessageFacebook.Objects;

public class AppSettings
{
    public FacebookSettings Facebook { get; set; } = default!;
}

public class FacebookSettings
{
    public string WebhookVerifyCode { get; set; } = default!;
    public string AppId { get; set; } = default!;
    public string AppSecret { get; set; } = default!;
}