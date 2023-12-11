using CelltechMessageFacebook.Objects.ResponseObjects;
using Microsoft.AspNetCore.SignalR;

namespace CelltechMessageFacebook.Hubs;

public class ChatHub : Hub
{
    public async Task NewMessage(Guid userId, MessageResponse message) =>
        await Clients.User(userId.ToString()).SendAsync("messageReceived", message);
}