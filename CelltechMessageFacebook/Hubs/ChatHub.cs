using CelltechMessageFacebook.Objects.ResponseObjects;
using Microsoft.AspNetCore.SignalR;

namespace CelltechMessageFacebook.Hubs;

public class ChatHub : Hub
{
    public readonly static Dictionary<string, string> ConnectionMappings = new();

    public override Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext()!.Request.Query["userId"].ToString();
        ConnectionMappings[userId] = Context.ConnectionId;
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.GetHttpContext()!.Request.Query["userId"].ToString();
        ConnectionMappings.Remove(userId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task NewMessage(Guid userId, MessageResponse message) {
        var connectionId = ConnectionMappings.GetValueOrDefault(userId.ToString());
        if (connectionId is not null) {
            await Clients.Client(connectionId).SendAsync("messageReceived", message);
        }
    }
}
