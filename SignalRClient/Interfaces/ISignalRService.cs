namespace SignalRClient.Interfaces;

public interface ISignalRService
{
    Action<string>? OnMessageReceived { get; set; }
    bool IsConnected { get; }
    Task ConnectOrDisconnect();
    void SendMessage(string message);
}