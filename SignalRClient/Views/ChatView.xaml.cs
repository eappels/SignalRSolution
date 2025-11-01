using SignalRClient.ViewModels;

namespace SignalRClient.Views;

public partial class ChatView : ContentPage
{
	public ChatView(ChatViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
    }
}