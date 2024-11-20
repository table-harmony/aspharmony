using Microsoft.AspNetCore.SignalR;
using WebSockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR()
    .AddJsonProtocol(options => {
        options.PayloadSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddLogging(logging => {
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder => {
        builder.WithOrigins("https://localhost:7089")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.MapPost("broadcast", async (string message, IHubContext<ChatHub, IChatClient> context) => {
    await context.Clients.All.ReceiveMessage(message);

    return Results.NoContent();
});

app.MapHub<ChatHub>("chat-hub");

app.Run();
