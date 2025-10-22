namespace Beholder.Views;

public partial class favoritesPage : ContentPage
{
    public favoritesPage(FavoritesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel.BindPage(this);
        Shell.SetNavBarIsVisible(this, false);
    }
}