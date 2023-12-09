using CelltechMessageFacebook.Domain;

namespace CelltechMessageFacebook.Objects.ResponseObjects;

public class MessageResponse : Message
{
    public List<MessageBlock> MessageBlocks { get; set; } = default!;
}