namespace Beholder.ViewModels;

public partial class ListPageViewModel : INotifyPropertyChanged
{
    readonly AppState _appState;
    readonly INavigationService _navigation;
    ContentPage? _page;
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

    public ListPageViewModel(INavigationService navigation, AppState appState)
    {
        OpenTeleprogramPageCommand = new Command<Int32>(OpenTeleprogramPage);
        ChannelSortCommand = new Command<FilterData>(ChannelSort);

        _navigation = navigation;
        _appState = appState;
        _appState.PropertyChanged += OnAppStatePropertyChanged;

        if (_appState.Channels == null)
        {
            IsBusy = true;
            Task.Run(async () => await _appState.LoadChannelsAsync());
        }
    }

    void OnAppStatePropertyChanged(Object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppState.Channels))
        {
            IsBusy = false;
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


        switch (data.State)
        {
            case FilterState.DescendingSort:
                {
                    switch (data.Text)
                    {
                        case "Номер":
                            {
                                _appState.SetChannels(Channels.OrderByDescending(channel => channel.number).ToList());
                                break;
                            }
                        case "Название":
                            {
                                _appState.SetChannels(Channels.OrderByDescending(channel => channel.name).ToList());
                                break;
                            }
                        case "Описание":
                            {
                                _appState.SetChannels(Channels.OrderByDescending(channel => channel.description).ToList());
                                break;
                            }
                    }
                    break;
                }
            case FilterState.AscendingSort:
                {
                    switch (data.Text)
                    {
                        case "Номер":
                            {
                                _appState.SetChannels(Channels.OrderBy(channel => channel.number).ToList());
                                break;
                            }
                        case "Название":
                            {
                                _appState.SetChannels(Channels.OrderBy(channel => channel.name).ToList());
                                break;
                            }
                        case "Описание":
                            {
                                _appState.SetChannels(Channels.OrderBy(channel => channel.description).ToList());
                                break;
                            }
                    }
                    break;
                }
            case FilterState.Idle:
                {
                    _appState.SetChannels(Channels.OrderBy(channel => channel.number).ToList());
                    break;
                }
            default: { break; }
        }

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
