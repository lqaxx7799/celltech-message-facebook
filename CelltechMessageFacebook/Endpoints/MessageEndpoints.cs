using CelltechMessageFacebook.Managers;
using CelltechMessageFacebook.Objects.RequestObjects;

namespace CelltechMessageFacebook.Endpoints;

public static class MessageEndpoints
{
    public static void AddMessageEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("message");

        group.MapGet("list", ([AsParameters] MessageListRequest request) =>
        {
            var messages = DataManager.Messages.Values
                .Where(x => x.ConversationId == request.ConversationId)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            return Results.Ok(messages);
        });

        group.MapPost("reply", () =>
        {
            return Results.Ok();
        });
    }
}