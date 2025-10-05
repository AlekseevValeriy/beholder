namespace Beholder.Views;

public partial class ListPage : ContentPage
{
    readonly FilterCoordinator _filterCoordinator = new();

    public ListPage(ListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel.BindPage(this);
        Shell.SetNavBarIsVisible(this, false);

        foreach (Filter item in FiltersLayout.Children.OfType<Filter>())
        {
            _filterCoordinator.Register(item);
        }
    }
}

public class FilterCoordinator
{
    readonly List<Filter> _filters = new();

    public void Register(Filter filter)
    {
        _filters.Add(filter);
        filter.BecameFocused += OnFilterFocused;
    }

    private void OnFilterFocused(Object sender)
    {
        foreach (var filter in _filters)
        {
            if (!ReferenceEquals(filter, sender))
            {
                filter.StateMachine.ToIdle();
            }
        }
    }
}