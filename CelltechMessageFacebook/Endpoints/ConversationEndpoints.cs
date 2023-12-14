using CelltechMessageFacebook.Domain;
using CelltechMessageFacebook.Managers;
using CelltechMessageFacebook.Objects.RequestObjects;
using CelltechMessageFacebook.Objects.ResponseObjects;

namespace CelltechMessageFacebook.Endpoints;

public static class ConversationEndpoints
{
    public static void AddConversationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("conversation");
        group.MapGet("list", ([AsParameters] ConversationListRequest request) =>
        {
            var conversations = DataManager.Conversations.Values
                .Where(x => x.UserIds.Contains(request.UserId))
                .Select(x =>
                {
                    var conversationResponse = new ConversationResponse
                    {
                        CreatedAt = x.CreatedAt,
                        CreatedBy = x.CreatedBy,
                        CustomerId = x.CustomerId,
                        FacebookPageId = x.FacebookPageId,
                        ModifiedAt = x.ModifiedAt,
                        Id = x.Id,
                        ModifiedBy = x.ModifiedBy,
                        Customer = DataManager.Users.Values.FirstOrDefault(c => c.Id == x.CustomerId && c.Type == UserType.Customer)
                    };
                    var lastMessage = DataManager.Messages.Values
                        .OrderByDescending(y => y.CreatedAt)
                        .FirstOrDefault();
                    if (lastMessage is not null)
                    {
                        var lastMessageBlocks = DataManager.MessageBlocks.Values
                            .Where(y => y.MessageId == lastMessage.Id)
                            .ToList();
                        var lastMessageResponse = new MessageResponse
                        {
                            Id = lastMessage.Id,
                            CreatedAt = lastMessage.CreatedAt,
                            ConversationId = lastMessage.ConversationId,
                            SenderId = lastMessage.SenderId,
                            CreatedBy = lastMessage.CreatedBy,
                            ModifiedAt = lastMessage.ModifiedAt,
                            ModifiedBy = lastMessage.ModifiedBy,
                            MessageBlocks = lastMessageBlocks
                        };
                        conversationResponse.LastMessage = lastMessageResponse;
                    }
                    
                    return conversationResponse;
                })
                .ToList();
            return Results.Ok(conversations);
        });
        
        group.MapGet("get", ([AsParameters] ConversationGetRequest request) =>
        {
            var conversation = DataManager.Conversations.Values
                .FirstOrDefault(x => x.Id == request.Id);
            return Results.Ok(conversation);
        });
    }
}