using System.Collections.Concurrent;
using CelltechMessageFacebook.Domain;

namespace CelltechMessageFacebook.Managers;

public class DataManager
{
    public ConcurrentDictionary<Guid, User> Users = new();
    public ConcurrentDictionary<Guid, FacebookPage> FacebookPages = new();
    public ConcurrentDictionary<Guid, Conversation> Conversations = new();
    public ConcurrentDictionary<Guid, Message> Messages = new();
    public ConcurrentDictionary<Guid, MessageBlock> MessageBlocks = new();
}