namespace Beholder.Views;

public partial class TeleprogramPage : ContentPage
{
    public TeleprogramPage(TeleprogramPageViewModel viewModel, Int32 channelId)
    {
        InitializeComponent();
        BindingContext = viewModel.BindChannelId(channelId).BindPage(this);
        Shell.SetNavBarIsVisible(this, false);

        Unloaded += OnDisappearing;
        //Disappearing += OnDisappearing;
    }

    private void OnDisappearing(Object? sender, EventArgs e)
    {
        if (BindingContext is not TeleprogramPageViewModel vm) return;

        vm.appState.ClearChannel();
        vm.appState.ClearSchedule();
    }
}