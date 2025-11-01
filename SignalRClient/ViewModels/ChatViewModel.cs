using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SignalRClient.Interfaces;
using System.Collections.ObjectModel;

namespace SignalRClient.ViewModels;

public partial class ChatViewModel : ObservableObject, IDisposable
{

    private readonly ISignalRService signalRService;
    public ObservableCollection<string> Messages { get; } = new();

    public ChatViewModel(ISignalRService signalRService)
    {
        this.signalRService = signalRService;
        this.signalRService.OnMessageReceived += (message) =>
        {
            Messages.Add(message);
        };
        ConnectButtonColor = Colors.Red;
    }

    [RelayCommand]
    public async Task Connect()
    {
        await signalRService.ConnectOrDisconnect();
        if (signalRService.IsConnected == true)
        {         
            ConnectButtonText = "Disconnect";
            ConnectButtonColor = Colors.Green;
        }
        else
        {
            ConnectButtonText = "Connect";
            ConnectButtonColor = Colors.Red;
        }
    }

    [RelayCommand]
    public async void SendMessage()
    {
        signalRService.SendMessage(MessageText);
    }

    public void Dispose()
    {
        this.signalRService.OnMessageReceived -= (message) =>
        {
            Messages.Add(message);
        };
    }

    [ObservableProperty]
    public string messageText;

    [ObservableProperty]
    public string connectButtonText = "Connect";

    [ObservableProperty]
    public Color connectButtonColor;
}