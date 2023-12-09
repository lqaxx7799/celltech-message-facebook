using CelltechMessageFacebook.Domain;
using CelltechMessageFacebook.Managers;
using CelltechMessageFacebook.Objects;
using CelltechMessageFacebook.Objects.RequestObjects;
using CelltechMessageFacebook.Objects.ResponseObjects;
using CelltechMessageFacebook.Services;
using Microsoft.AspNetCore.Mvc;

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
                .Select(x =>
                {
                    var message = new MessageResponse
                    {
                        Id = x.Id,
                        CreatedAt = x.CreatedAt,
                        ConversationId = x.ConversationId,
                        SenderId = x.SenderId,
                        CreatedBy = x.CreatedBy,
                        ModifiedAt = x.ModifiedAt,
                        ModifiedBy = x.ModifiedBy,
                        MessageBlocks = DataManager.MessageBlocks.Values.Where(y => y.MessageId == x.Id).ToList()
                    };
                    return message;
                })
                .ToList();
            return Results.Ok(messages);
        });

        group.MapPost("reply", (
            [FromServices] IFacebookService facebookService,
            [FromBody] MessageReplyRequest request) =>
        {
            var message = new Message
            {
                CreatedAt = DateTimeOffset.Now,
                SenderId = request.SenderId,
                ConversationId = request.ConversationId,
            };
            var messageBlock = new MessageBlock
            {
                Content = request.Content,
                ContentType = "text/html",
                MessageId = message.Id,
                CreatedAt = DateTimeOffset.Now
            };
            DataManager.Messages[message.Id] = message;
            DataManager.MessageBlocks[messageBlock.Id] = messageBlock;
            return Results.Ok(message);
        });
    }
}