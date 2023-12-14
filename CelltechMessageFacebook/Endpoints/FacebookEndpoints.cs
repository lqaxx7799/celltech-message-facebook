using CelltechMessageFacebook.Domain;
using CelltechMessageFacebook.Hubs;
using CelltechMessageFacebook.Managers;
using CelltechMessageFacebook.Objects;
using CelltechMessageFacebook.Objects.FacebookObjects;
using CelltechMessageFacebook.Objects.RequestObjects;
using CelltechMessageFacebook.Objects.ResponseObjects;
using CelltechMessageFacebook.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CelltechMessageFacebook.Endpoints;

public static class FacebookEndpoints
{
    public static void AddFacebookEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("facebook");
        
        // hook - start
        group.MapGet("hook", (
            [FromServices] AppSettings appSettings,
            [FromQuery(Name = "hub.mode")] string mode = "",
            [FromQuery(Name = "hub.challenge")] string challenge = "",
            [FromQuery(Name = "hub.verify_token")] string verifyToken = "") =>
        {
            if (verifyToken != appSettings.Facebook.WebhookVerifyCode)
            {
                return Results.BadRequest("Verify code not matched");
            }
            int.TryParse(challenge, out var result);
            return Results.Ok(result);
        });

        group.MapPost("hook", async (
            [FromServices] IFacebookService facebookService,
            [FromServices] IHubContext<ChatHub> chatHubContext,
            [FromBody] FacebookHookRequest request) =>
        {
            foreach (var entry in request.Entry)
            {
                foreach (var messaging in entry.Messaging)
                {
                    var isPageSender = entry.Id == messaging.Sender.Id;
                    if (isPageSender)
                    {
                        var senders = DataManager.FacebookPages.Values.Where(x => x.PageId == messaging.Sender.Id).ToList();
                        foreach (var senderPage in senders)
                        {
                            var senderUser = DataManager.Users.Values.FirstOrDefault(x =>
                                x.Id == senderPage.UserId && x.Type == UserType.Agent);
                            if (senderUser is null)
                            {
                                continue;
                            }

                            var recipientUser = DataManager.Users.Values.FirstOrDefault(x =>
                                x.FacebookAccountId == messaging.Recipient.Id && x.Type == UserType.Customer &&
                                x.UserOwnerId == senderUser.Id);
                            // create new customer user if not exist
                            if (recipientUser is null)
                            {
                                var recipientFacebookUser =
                                    await facebookService.GetNode<FacebookAccountResponse>(messaging.Recipient.Id, senderPage.AccessToken!);
                                recipientUser = new User
                                {
                                    CreatedAt = DateTimeOffset.Now,
                                    FacebookAccountId = messaging.Recipient.Id,
                                    Type = UserType.Customer,
                                    UserName = recipientFacebookUser.FirstName + " " + recipientFacebookUser.LastName,
                                    UserOwnerId = senderUser.Id,
                                };
                                DataManager.Users[recipientUser.Id] = recipientUser;
                            }

                            var conversation = DataManager.Conversations.Values.FirstOrDefault(x =>
                                x.UserIds.Contains(recipientUser.Id) && x.UserIds.Contains(senderUser.Id));
                            if (conversation is null)
                            {
                                conversation = new Conversation
                                {
                                    CreatedAt = DateTimeOffset.Now,
                                    CustomerId = recipientUser.Id,
                                    FacebookPageId = senderPage.Id,
                                    UserIds = new List<Guid> { recipientUser.Id, senderUser.Id }
                                };
                                DataManager.Conversations[conversation.Id] = conversation;
                            }
                            var message = new Message
                            {
                                CreatedAt = DateTimeOffset.Now,
                                SenderId = senderUser.Id,
                                ConversationId = conversation.Id,
                                FacebookMessageId = messaging.Message.Mid
                            };
                            var messageBlock = new MessageBlock
                            {
                                Content = messaging.Message.Text,
                                ContentType = "text/html",
                                MessageId = message.Id,
                                CreatedAt = DateTimeOffset.Now
                            };
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

                            var conversationResponse = new ConversationResponse
                            {
                                CreatedAt = conversation.CreatedAt,
                                CreatedBy = conversation.CreatedBy,
                                CustomerId = conversation.CustomerId,
                                FacebookPageId = conversation.FacebookPageId,
                                ModifiedAt = conversation.ModifiedAt,
                                Id = conversation.Id,
                                ModifiedBy = conversation.ModifiedBy,
                                Customer = DataManager.Users.Values.FirstOrDefault(c => c.Id == conversation.CustomerId && c.Type == UserType.Customer),
                                LastMessage = messageResponse
                            };

                            // TODO: send signalR
                            var connectionId = ChatHub.ConnectionMappings.GetValueOrDefault(senderUser.Id.ToString());
                            if (connectionId is not null) {
                                await chatHubContext.Clients.Client(connectionId).SendAsync("messageReceived", messageResponse);
                                await chatHubContext.Clients.Client(connectionId).SendAsync("newConversation", conversationResponse);
                            }
                        }
                    }
                    else
                    {
                        var recipients = DataManager.FacebookPages.Values.Where(x => x.PageId == messaging.Recipient.Id).ToList();
                        foreach (var recipientPage in recipients)
                        {
                            var recipientUser = DataManager.Users.Values.FirstOrDefault(x =>
                                x.Id == recipientPage.UserId && x.Type == UserType.Agent);
                            if (recipientUser is null)
                            {
                                continue;
                            }

                            var senderUser = DataManager.Users.Values.FirstOrDefault(x =>
                                x.FacebookAccountId == messaging.Sender.Id && x.Type == UserType.Customer &&
                                x.UserOwnerId == recipientUser.Id);
                            // create new customer user if not exist
                            if (senderUser is null)
                            {
                                var senderFacebookUser =
                                    await facebookService.GetNode<FacebookAccountResponse>(messaging.Sender.Id, recipientPage.AccessToken!);
                                senderUser = new User
                                {
                                    CreatedAt = DateTimeOffset.Now,
                                    FacebookAccountId = messaging.Sender.Id,
                                    Type = UserType.Customer,
                                    UserName = senderFacebookUser.FirstName + " " + senderFacebookUser.LastName,
                                    UserOwnerId = recipientUser.Id,
                                };
                                DataManager.Users[senderUser.Id] = senderUser;
                            }

                            var conversation = DataManager.Conversations.Values.FirstOrDefault(x =>
                                x.UserIds.Contains(recipientUser.Id) && x.UserIds.Contains(senderUser.Id));
                            if (conversation is null)
                            {
                                conversation = new Conversation
                                {
                                    CreatedAt = DateTimeOffset.Now,
                                    CustomerId = senderUser.Id,
                                    FacebookPageId = recipientPage.Id,
                                    UserIds = new List<Guid> { recipientUser.Id, senderUser.Id }
                                };
                                DataManager.Conversations[conversation.Id] = conversation;
                            }
                            var message = new Message
                            {
                                CreatedAt = DateTimeOffset.Now,
                                SenderId = senderUser.Id,
                                ConversationId = conversation.Id,
                                FacebookMessageId = messaging.Message.Mid
                            };
                            var messageBlock = new MessageBlock
                            {
                                Content = messaging.Message.Text,
                                ContentType = "text/html",
                                MessageId = message.Id,
                                CreatedAt = DateTimeOffset.Now
                            };
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

                            var conversationResponse = new ConversationResponse
                            {
                                CreatedAt = conversation.CreatedAt,
                                CreatedBy = conversation.CreatedBy,
                                CustomerId = conversation.CustomerId,
                                FacebookPageId = conversation.FacebookPageId,
                                ModifiedAt = conversation.ModifiedAt,
                                Id = conversation.Id,
                                ModifiedBy = conversation.ModifiedBy,
                                Customer = DataManager.Users.Values.FirstOrDefault(c => c.Id == conversation.CustomerId && c.Type == UserType.Customer),
                                LastMessage = messageResponse
                            };

                            // TODO: send signalR
                            var connectionId = ChatHub.ConnectionMappings.GetValueOrDefault(recipientUser.Id.ToString());
                            if (connectionId is not null) {
                                await chatHubContext.Clients.Client(connectionId).SendAsync("messageReceived", messageResponse);
                                await chatHubContext.Clients.Client(connectionId).SendAsync("newConversation", conversationResponse);
                            }
                        }
                    }
                }
            }

            return Results.Ok();
        });
        
        // hook - end

        group.MapGet("pages", async ([FromServices] IFacebookService facebookService, [FromQuery] string accessToken) =>
        {
            var pages = await facebookService.GetPages(accessToken);
            return Results.Ok(pages);
        });
    }

}