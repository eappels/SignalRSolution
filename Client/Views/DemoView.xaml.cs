using Client.ViewModels;

namespace Client.Views;

public partial class DemoView : ContentPage
{
    public DemoView(DemoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}