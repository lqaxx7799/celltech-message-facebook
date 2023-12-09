using System.Collections.Concurrent;
using CelltechMessageFacebook.Domain;

namespace CelltechMessageFacebook.Managers;

public class DataManager
{
    public static ConcurrentDictionary<Guid, User> Users = new();
    public static ConcurrentDictionary<Guid, FacebookPage> FacebookPages = new();
    public static ConcurrentDictionary<Guid, Conversation> Conversations = new();
    public static ConcurrentDictionary<Guid, Message> Messages = new();
    public static ConcurrentDictionary<Guid, MessageBlock> MessageBlocks = new();
}