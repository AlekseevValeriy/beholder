namespace Beholder.ViewModels;
public partial class SearchPageViewModel : INotifyPropertyChanged
{
    readonly AppState _appState;
    readonly INavigationService _navigation;

    ContentPage? _page;
    String _searchQuery = "";
    Boolean _isBusy = false;

    public Boolean IsBusy
    {
        get => _isBusy;
        set
        {
            if (value != _isBusy)
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
    }

    public List<ChannelResponse>? ChannelsQueryResult
    {
        get
        {
            if (_appState.ChannelsQueryResult is not null && _appState.ChannelsQueryResult.IsSuccess && _appState.ChannelsQueryResult.Content is not null)
                return _appState.ChannelsQueryResult.Content;
            return null;

        }
    }

    public String SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (value != _searchQuery)
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SearchCommand { get; set; }
    public ICommand OpenTeleprogramPageCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public SearchPageViewModel(INavigationService navigation, AppState appState)
    {
        SearchCommand = new Command(Search);
        OpenTeleprogramPageCommand = new Command<Int32>(OpenTeleprogramPage);

        _navigation = navigation;
        _appState = appState;
        _appState.PropertyChanged += OnAppStatePropertyChanged;

    }

    void OnAppStatePropertyChanged(Object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppState.ChannelsQueryResult))
        {
            IsBusy = false;
            OnPropertyChanged(nameof(ChannelsQueryResult));
        }
    }

    public SearchPageViewModel BindPage(ContentPage page)
    {
        _page = page;
        return this;
    }

    async void OpenTeleprogramPage(Int32 channelId)
    {
        if (_page is null) return;

        await _navigation.NavigateToTeleprogramPageAsync(_page, channelId);
    }

    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    async void Search()
    {
        IsBusy = true;
        await _appState.LoadChannelsByQueryAsync(SearchQuery);
    }
}
