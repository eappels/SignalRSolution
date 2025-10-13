using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;

namespace Client.ViewModels;

public partial class DemoViewModel : ObservableObject
{

    private readonly HubConnection hubConnection;
    public ObservableCollection<string> Messages { get; } = new();

    public DemoViewModel()
    {
        ConnectButtonColor = Colors.Red;
        hubConnection = new HubConnectionBuilder()
            .WithUrl($"https://localhost:7146/chatHub")
            .Build();
        hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            Messages.Add($"{user}: {message}");
        });
    }

    [RelayCommand]
    public async Task Connect()
    {
        if (hubConnection.State == HubConnectionState.Disconnected)
        {
            await hubConnection.StartAsync();
            ConnectButtonText = "Disconnect";
            ConnectButtonColor = Colors.Green;
        }
        else
        {
            await hubConnection.StopAsync();
            ConnectButtonText = "Connect";
            ConnectButtonColor = Colors.Red;
        }
    }

    [RelayCommand]
    public async void SendMessage()
    {
        if (hubConnection.State == HubConnectionState.Connected && !string.IsNullOrWhiteSpace(MessageText))
            await hubConnection.SendAsync("SendMessage", "DesktopApp", MessageText);
    }

    [ObservableProperty]
    public string messageText;

    [ObservableProperty]
    public string connectButtonText = "Connect";

    [ObservableProperty]
    public Color connectButtonColor;
}