namespace Beholder.Views;

public partial class AccountPage : ContentPage
{
    public AccountPage(AccountPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel.BindPage(this);
        Shell.SetNavBarIsVisible(this, false);
    }
}