using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace WebSockets;

public class Message {
    public string Sender { get; set; } = "Server";
    public string Content { get; set; } = "";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public interface IChatClient {
    Task ReceiveMessage(Message message);
}

public sealed class ChatHub : Hub<IChatClient> {
    public override async Task OnConnectedAsync() {
        Message message = new() {
            Content = "User has joined the chat",
        };

        await Clients.All.ReceiveMessage(message);
    }

    public override async Task OnDisconnectedAsync(Exception? exception) {
        Message message = new() {
            Content = "User has left the chat"
        };

        await Clients.All.ReceiveMessage(message);
    }

    public async Task SendMessage(string message) {
        Message parsedMessage = JsonSerializer.Deserialize<Message>(message) 
            ?? throw new HubException("Message could not be parsed");

        await Clients.All.ReceiveMessage(parsedMessage);
    }
}
