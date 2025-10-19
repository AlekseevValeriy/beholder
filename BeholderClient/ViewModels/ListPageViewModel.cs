namespace Beholder.ViewModels;

public partial class ListPageViewModel : INotifyPropertyChanged
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
    public List<ChannelResponse>? Channels
    {
        get
        {
            if (_appState.Channels is not null && _appState.Channels.IsSuccess && _appState.Channels.Content is not null)
                return _appState.Channels.Content;
            return null;

        }
    }

    public ICommand OpenTeleprogramPageCommand { get; set; }
    public ICommand ChannelSortCommand { get; set; }
    public ICommand LoadDataCommand { get; set; }

    public ListPageViewModel(INavigationService navigation, AppState appState)
    {
        OpenTeleprogramPageCommand = new Command<Int32>(OpenTeleprogramPage);
        ChannelSortCommand = new Command<FilterData>(ChannelSort);
        LoadDataCommand = new Command(LoadData);

        _navigation = navigation;
        _appState = appState;
        _appState.PropertyChanged += OnAppStatePropertyChanged;

        if (_appState.Channels == null) LoadData();
    }

    async public void LoadData()
    {
        IsLoadSucces = true;
        IsBusy = true;
        ProblemContent = Problem.None;
        await _appState.LoadChannelsAsync();
    }

    void OnAppStatePropertyChanged(Object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppState.Channels))
        {
            IsBusy = false;

            if (Channels is null)
            {
                ProblemContent = ProblemHandleHelper.ProblemHandle<List<ChannelResponse>>(_appState.Channels);
                IsLoadSucces = false;
                return;
            }

            IsLoadSucces = true;
            OnPropertyChanged(nameof(Channels));
        }
    }

    public ListPageViewModel BindPage(ContentPage page)
    {
        _page = page;
        return this;
    }

    void ChannelSort(FilterData data)
    {
        if (Channels is null) return;

        _appState.SetChannels((data switch
        {
            (FilterState.DescendingSort, "Номер") => Channels.OrderByDescending(channel => channel.number),
            (FilterState.DescendingSort, "Название") => Channels.OrderByDescending(channel => channel.name),
            (FilterState.DescendingSort, "Описание") => Channels.OrderByDescending(channel => channel.description),
            (FilterState.AscendingSort, "Номер") or (FilterState.Idle, _) => Channels.OrderBy(channel => channel.number),
            (FilterState.AscendingSort, "Название") => Channels.OrderBy(channel => channel.name),
            (FilterState.AscendingSort, "Описание") => Channels.OrderBy(channel => channel.description),
            _ => Channels.Order()
        }).ToList());
    }

    async void OpenTeleprogramPage(Int32 channelId)
    {
        if (_page is null) return;

        await _navigation.NavigateToTeleprogramPageAsync(_page, channelId);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
