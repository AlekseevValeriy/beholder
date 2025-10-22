namespace Beholder.Views;

public partial class TeleprogramPage : ContentPage
{
    public TeleprogramPage(TeleprogramPageViewModel viewModel, Int32 channelId)
    {
        InitializeComponent();
        BindingContext = viewModel.BindPage(this);
        (BindingContext as TeleprogramPageViewModel)!.LoadData(channelId);
        Shell.SetNavBarIsVisible(this, false);
    }
}