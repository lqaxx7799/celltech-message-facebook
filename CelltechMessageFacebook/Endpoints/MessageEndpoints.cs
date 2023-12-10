using CelltechMessageFacebook.Domain;
using CelltechMessageFacebook.Managers;
using CelltechMessageFacebook.Objects;
using CelltechMessageFacebook.Objects.FacebookObjects;
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

        group.MapPost("reply", async (
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

            var conversation = DataManager.Conversations.Values.FirstOrDefault(x => x.Id == request.ConversationId);
            var senderPage =
                DataManager.FacebookPages.Values.FirstOrDefault(x => x.Id == conversation?.FacebookPageId);
            var recipientUser = DataManager.Users.Values.FirstOrDefault(x => x.Id == conversation?.CustomerId && x.Type == UserType.Customer);
            var messageRequest = new FacebookSendMessageRequest
            {
                Recipient = new FacebookSendMessageRecipientRequest
                {
                    Id = recipientUser?.FacebookAccountId!,
                },
                Message = new FacebookSendMessageMessageRequest
                {
                    Text = request.Content
                },
                MessageType = "RESPONSE"
            };
            var facebookMessageResponse = await facebookService.SendMessage(messageRequest, senderPage?.PageId!, senderPage?.AccessToken!);
            message.FacebookMessageId = facebookMessageResponse.MessageId;

            DataManager.Messages[message.Id] = message;
            DataManager.MessageBlocks[messageBlock.Id] = messageBlock;

            var messageResponse = new MessageResponse
            {
                Id = message.Id,
                CreatedAt = message.CreatedAt,
                ConversationId = message.ConversationId,
                SenderId = message.SenderId,
                CreatedBy = message.CreatedBy,
                ModifiedAt = message.ModifiedAt,
                ModifiedBy = message.ModifiedBy,
                MessageBlocks = new List<MessageBlock> { messageBlock }
            };
            return Results.Ok(messageResponse);
        });
    }
}