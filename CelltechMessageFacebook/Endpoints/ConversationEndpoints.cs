using CelltechMessageFacebook.Managers;
using CelltechMessageFacebook.Objects.RequestObjects;

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
                .ToList();
            return Results.Ok(conversations);
        });
    }
}