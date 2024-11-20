using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace WebSockets;

public class Message {
    public string Sender { get; set; } = "Server";
    public string Content { get; set; } = "";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public interface IChatClient {
    Task ReceiveMessage(string message);
}

public sealed class ChatHub : Hub<IChatClient> {
    public override async Task OnConnectedAsync() {
        Message message = new() {
            Content = $"User {Context.ConnectionId} has joined the chat",
        };

        await Clients.All.ReceiveMessage(JsonSerializer.Serialize(message));
    }

    public override async Task OnDisconnectedAsync(Exception? exception) {
        Message message = new() {
            Content = $"User {Context.ConnectionId} has left the chat"
        };

        await Clients.All.ReceiveMessage(JsonSerializer.Serialize(message));
    }

    public async Task SendMessage(string content) {
        if (string.IsNullOrWhiteSpace(content)) {
            throw new HubException("Message content cannot be empty");
        }

        Message message = new() {
            Sender = Context.ConnectionId,
            Content = content
        };

        await Clients.All.ReceiveMessage(JsonSerializer.Serialize(message));
    }

    public async Task SendPrivateMessage(string targetUserId, string content) {
        Message message = new() {
            Sender = Context.ConnectionId,
            Content = content
        };

        await Clients.User(targetUserId).ReceiveMessage(JsonSerializer.Serialize(message));
    }
}
