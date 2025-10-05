namespace Beholder.Views;

public partial class AuthorizationPage : ContentPage
{
    public AuthorizationPage(AuthorizationPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel.BindPage(this);
        Shell.SetNavBarIsVisible(this, false);
    }
}