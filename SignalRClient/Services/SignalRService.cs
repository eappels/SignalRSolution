using Microsoft.AspNetCore.SignalR.Client;
using SignalRClient.Interfaces;

namespace SignalRClient.Services;

public class SignalRService : ISignalRService
{

    private readonly HubConnection hubConnection;
    private string username = "DesktopAppUser";

    public SignalRService()
    {
        hubConnection = new HubConnectionBuilder()
                            .WithUrl($"https://localhost:7115/chatHub")
                            .Build();
        hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            OnMessageReceived?.Invoke($"{user}: {message}");
        });
    }

    public async Task ConnectOrDisconnect()
    {
        int i = 0;
        if (hubConnection.State == HubConnectionState.Disconnected)
        {
            await hubConnection.StartAsync();
        }
        else
        {
            await hubConnection.StopAsync();
        }
    }

    public async void SendMessage(string message)
    {
        if (hubConnection.State == HubConnectionState.Connected && !string.IsNullOrWhiteSpace(message))
            await hubConnection.SendAsync("SendMessage", username, message);
    }

    public bool IsConnected
    {
        get { return hubConnection.State == HubConnectionState.Connected; }
    }

    public Action<string>? OnMessageReceived { get; set; }
}