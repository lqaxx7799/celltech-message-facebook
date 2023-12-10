using CelltechMessageFacebook.Objects.FacebookObjects;

namespace CelltechMessageFacebook.Objects.RequestObjects;

public class FacebookHookRequest
{
    public string Object { get; set; } = default!;
    public List<FacebookHookEntry> Entry { get; set; } = default!;
}