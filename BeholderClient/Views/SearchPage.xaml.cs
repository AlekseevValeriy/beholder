namespace Beholder.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel.BindPage(this);
        Shell.SetNavBarIsVisible(this, false);
    }
}
