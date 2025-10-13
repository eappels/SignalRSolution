using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs;

public class ChatHub : Hub
{

    private Dictionary<string, string> Connections = new();

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"User connected: {Context.User.Identity.Name}");
        await Clients.All.SendAsync("ReceiveMessage", "System:", $"User connected: {Context.User.Identity.Name}");
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string user, string message)
    {
        Console.WriteLine($"Received message from {user}: {message}");
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"User disconnected: {Context.User.Identity.Name}");
        await Clients.All.SendAsync("ReceiveMessage", "System:", $"User disconnected: {Context.User.Identity.Name}");
        await base.OnDisconnectedAsync(exception);
    }
}