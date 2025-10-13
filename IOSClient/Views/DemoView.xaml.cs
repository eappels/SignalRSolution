using IOSClient.ViewModels;

namespace IOSClient.Views;

public partial class DemoView : ContentPage
{
    public DemoView(DemoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}