using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ChatService>();

var app = builder.Build();
app.UseWebSockets();

app.MapGet("/", async (HttpContext context, ChatService chatService) => {
    if (context.WebSockets.IsWebSocketRequest) {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await chatService.HandleWebSocketConnection(webSocket);
    } else {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Expected a WebSocket request");
    }
});

app.Run();

public class ChatService {
    private readonly List<WebSocket> _sockets = [];
    private readonly Dictionary<WebSocket, string> _userNames = [];

    public async Task HandleWebSocketConnection(WebSocket socket) {
        _sockets.Add(socket);
        var buffer = new byte[1024 * 2];

        try {
            while (socket.State == WebSocketState.Open) {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), default);

                if (result.MessageType == WebSocketMessageType.Close) {
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer[..result.Count]);
                var data = JsonConvert.DeserializeObject<dynamic>(message);

                if (data?.type == "join") {
                    _userNames[socket] = data.userName;
                    await BroadcastMessage($"{data.userName} joined the chat", "System");
                } else if (data?.type == "chat") {
                    await BroadcastMessage(data.message.ToString(), _userNames[socket]);
                }
            }
        } catch (WebSocketException) {
        } finally {
            if (_sockets.Contains(socket)) {
                _sockets.Remove(socket);
                if (_userNames.TryGetValue(socket, out string? value)) {
                    var userName = value;
                    _userNames.Remove(socket);
                    await BroadcastMessage($"{userName} left the chat", "System");
                }
            }
        }
    }

    private async Task BroadcastMessage(string message, string sender) {
        var payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new {
            userName = sender,
            message
        }));

        foreach (var socket in _sockets.ToList()) {
            if (socket.State == WebSocketState.Open) {
                await socket.SendAsync(payload, WebSocketMessageType.Text, true, default);
            }
        }
    }
}
