using Microsoft.AspNetCore.SignalR;

namespace WebSockets;

public interface IChatClient {
    Task ReceiveMessage(string message);
}

public sealed class ChatHub : Hub<IChatClient> {
    public override async Task OnConnectedAsync() {
        await Clients.All.ReceiveMessage($"{Context.ConnectionId} has joined");
    }

    public override async Task OnDisconnectedAsync(Exception? exception) {
        await Clients.All.ReceiveMessage($"{Context.ConnectionId} has left");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string message) {
        await Clients.All.ReceiveMessage($"{Context.ConnectionId}: {message}");
    }
}
