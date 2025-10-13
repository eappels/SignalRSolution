using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace IOSClient.ViewModels;

public partial class DemoViewModel : ObservableObject
{

    private readonly HubConnection hubConnection;
    public ObservableCollection<string> Messages { get; } = new();

    public DemoViewModel()
    {
        MessageText = $"This is a test from my iPhone {DateTime.Now}";
        ConnectButtonColor = Colors.Red;
        hubConnection = new HubConnectionBuilder()
#if DEBUG
            .WithUrl($"https://192.168.10.100:7146/chatHub", (opts) =>
            {
                opts.HttpMessageHandlerFactory = (message) =>
                {
                    if (message is HttpClientHandler clientHandler)
                        clientHandler.ServerCertificateCustomValidationCallback +=
                            (sender, certificate, chain, sslPolicyErrors) => { return true; };
                    return message;
                };
            })
#else
            .WithUrl($"https://192.168.10.100:7146/chatHub")
#endif
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
            await hubConnection.SendAsync("SendMessage", "iPhone", MessageText);
    }

    [ObservableProperty]
    public string messageText;

    [ObservableProperty]
    public string connectButtonText = "Connect";

    [ObservableProperty]
    public Color connectButtonColor;
}