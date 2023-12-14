using CelltechMessageFacebook.Domain;

namespace CelltechMessageFacebook.Objects.ResponseObjects;
public class ConversationResponse : Conversation
{
    public MessageResponse? LastMessage { get; set; }
    public User? Customer { get; set; }
}
