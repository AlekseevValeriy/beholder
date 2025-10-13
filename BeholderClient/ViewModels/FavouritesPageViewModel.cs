namespace Beholder.ViewModels;

public partial class FavoritesPageViewModel : INotifyPropertyChanged
{
    readonly AppState _appState;
    readonly INavigationService _navigation;
    ContentPage? _page;
    Boolean _isBusy = false;
    Boolean _isLoadSucces = true;
    Problem _problemContent = Problem.None;

    public Problem ProblemContent
    {
        get => _problemContent;
        set
        {
            if (value != _problemContent)
            {
                _problemContent = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean IsLoadSucces
    {
        get => _isLoadSucces;
        set
        {
            if (value != _isLoadSucces)
            {
                _isLoadSucces = value;
                OnPropertyChanged();
            }
        }
    }
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

    public List<FavoriteResponse>? Favorites
    {
        get
        {
            if (_appState.Favorites is not null && _appState.Favorites.IsSuccess && _appState.Favorites.Content is not null)
                return _appState.Favorites.Content;
            return null;
        }
    }
    public String UserName
    {
        get
        {
            if (_appState.User is null || _appState.User.HasProblem || _appState.User.Content is null || _appState.User.Content.id == -1) return "Гость";
            return _appState.User.Content.login;
        }
    }

    public ICommand ToAuthorizationCommand { get; set; }
    public ICommand OpenTeleprogramPageCommand { get; set; }

    public FavoritesPageViewModel(IDataLoaderService dataLoader, INavigationService navigationService, AppState appState)
    {
        ToAuthorizationCommand = new Command(ToAuthorization);
        OpenTeleprogramPageCommand = new Command<Int32>(OpenTeleprogramPage);

        _navigation = navigationService;
        _appState = appState;
        _appState.PropertyChanged += OnAppStatePropertyChanged;

        if (_appState.Favorites == null) LoadData();
    }

    async void LoadData()
    {
        IsLoadSucces = true;
        IsBusy = true;
        ProblemContent = Problem.None;
        Int32 id;
        if (_appState.User is null || _appState.User.HasProblem || _appState.User.Content is null) id = -1;
        else id = _appState.User.Content.id;

        await _appState.LoadFavoritesAsync(id);
    }

    async void OnAppStatePropertyChanged(Object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppState.Favorites))
        {
            IsBusy = false;

            if (Favorites is null)
            {
                ProblemContent = await ProblemHandleHelper.ProblemHandle<List<FavoriteResponse>>(_appState.Favorites, _page);
                IsLoadSucces = false;
                return;
            }
            IsLoadSucces = true;
            OnPropertyChanged(nameof(Favorites));
        }
        if (e.PropertyName == nameof(AppState.User))
        {
            OnPropertyChanged(nameof(UserName));
        }
    }

    public FavoritesPageViewModel BindPage(ContentPage page)
    {
        _page = page;
        return this;
    }

    async void OpenTeleprogramPage(Int32 channelId)
    {
        if (_page is null) return;

        await _navigation.NavigateToTeleprogramPageAsync(_page, channelId);
    }

    async void ToAuthorization()
    {
        if (_page is null || IsBusy == true || _appState?.User?.HttpError is System.Net.HttpStatusCode.BadGateway) return;

        if (_appState?.User is null)
        {
            await _navigation.NavigateToAuthorizationPageAsync(_page);
        }
        else
        {
            await _navigation.NavigateToAccountPageAsync(_page);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
