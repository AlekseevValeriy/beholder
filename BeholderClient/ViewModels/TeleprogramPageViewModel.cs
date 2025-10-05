namespace Beholder.ViewModels;

public partial class TeleprogramPageViewModel : INotifyPropertyChanged
{
    public readonly AppState appState;

    ContentPage? _page;
    Int32 _channelId;
    Boolean _tagsContains = false;
    Boolean _schudleContains = false;
    Boolean _isFavorite = false;
    String _pastButtonText = "";
    DateTime _pastDate = DateTime.Today.Date;
    String _futureButtonText = "";
    DateTime _futureDate = DateTime.Today.Date;
    ObservableCollection<ScheduleResponseGroup>? _groupedSchedules;
    Boolean _isAuthorized = false;
    Boolean _isLoaded = false;

    public String PastButtonText
    {
        get => _pastButtonText;
        set
        {
            if (value != _pastButtonText)
            {
                _pastButtonText = value;
                OnPropertyChanged();
            }
        }
    }
    public String FutureButtonText
    {
        get => _futureButtonText;
        set
        {
            if (value != _futureButtonText)
            {
                _futureButtonText = value;
                OnPropertyChanged();
            }
        }
    }
    public ObservableCollection<ScheduleResponseGroup>? GroupedSchedules
    {
        get => _groupedSchedules;
        set
        {
            if (value != _groupedSchedules)
            {
                _groupedSchedules = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean IsFavorite
    {
        get => _isFavorite;
        set
        {
            if (value != _isFavorite)
            {
                _isFavorite = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean IsLoaded
    {
        get => _isLoaded;
        set
        {
            if (value != _isLoaded)
            {
                _isLoaded = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean IsAuthorized
    {
        get => _isAuthorized;
        set
        {
            if (value != _isAuthorized)
            {
                _isAuthorized = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean TagsContaing
    {
        get => _tagsContains;
        set
        {
            if (value != _tagsContains)
            {
                _tagsContains = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean SchudleContains
    {
        get => _schudleContains;
        set
        {
            if (value != _schudleContains)
            {
                _schudleContains = value;
                OnPropertyChanged();
            }
        }
    }
    public ChannelResponse? Channel
    { 
        get
        {
            if (appState.Channel is not null && appState.Channel.IsSuccess && appState.Channel.Content is not null)
                return appState.Channel.Content;
            return null;
        }
    }

    public ICommand ToBackCommand { get; set; }
    public ICommand ClickFavoriteButtonCommand { get; set; }
    public ICommand PastButtonClickCommand { get; set; }
    public ICommand FutureButtonClickCommand { get; set; }

    public TeleprogramPageViewModel(AppState appState)
    {
        PastButtonClickCommand = new Command(PastButtonClick);
        FutureButtonClickCommand = new Command(FutureButtonClick);
        ToBackCommand = new Command(ToBack);
        ClickFavoriteButtonCommand = new Command(ClickFavoriteButton);

        this.appState = appState;
        this.appState.PropertyChanged += OnAppStatePropertyChanged;

        if (this.appState.Channel == null)
        {
            Task.Run(async () => await this.appState.LoadChannelAsync(_channelId));
            Task.Run(async () =>
            {
                if (appState.User is not null && appState.User.IsSuccess && appState.User.Content is not null)
                {
                    IsAuthorized = true;

                    var result = await this.appState.hasFavoriteAsync(appState.User.Content.id, _channelId);
                    if (result.IsSuccess) IsFavorite = result.Content;
                }
            });
            Task.Run(() => AddSchudle(DateTime.Today, ScheduleLoadPosition.Just));
        }

        MoveNextFutureText();
        MoveNextPastText();

        if (GroupedSchedules is not null) SchudleContains = true;
    }

    void OnAppStatePropertyChanged(Object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppState.Channel))
        {
            OnPropertyChanged(nameof(Channel));

            if (Channel is null)
            {
                IsLoaded = false;
                return;
            }

            TagsContaing = Channel.tags is null ? false : true;
            IsLoaded = true;
        }
        if (e.PropertyName == nameof(AppState.Schedule))
        {
            if (appState.Schedule is null || appState.Schedule.HasProblem || appState.Schedule.Content is null)
            {
                SchudleContains = false;
                return;
            }

            GroupedSchedules = new(appState.Schedule.Content.GroupBy(s => s.start_time.ToString("d")).Select(g => new ScheduleResponseGroup(g.Key, new(g))));
            SchudleContains = true;
        }
    }

    public TeleprogramPageViewModel BindPage(ContentPage page)
    {
        _page = page;
        return this;
    }

    async void ToBack()
    {
        if (_page is null) return;

        await _page.Navigation.PopAsync();
    }

    void MoveNextPastText()
    {
        _pastDate = _pastDate.AddDays(-1);
        PastButtonText = $"Программы на {_pastDate.ToString("d")}";
    }

    void MoveNextFutureText()
    {
        _futureDate = _futureDate.AddDays(1);
        FutureButtonText = $"Программы на {_futureDate.ToString("d")}";
    }

    void PastButtonClick()
    {
        AddSchudle(_pastDate, ScheduleLoadPosition.Past);

        MoveNextPastText();
    }
    void FutureButtonClick()
    {
        AddSchudle(_futureDate, ScheduleLoadPosition.Future);

        MoveNextFutureText();
    }

    async void AddSchudle(DateTime date, ScheduleLoadPosition postion)
    {
        await appState.LoadScheduleAsync(_channelId, date);
    }


    public TeleprogramPageViewModel BindChannelId(Int32 channelId)
    {
        _channelId = channelId;
        return this;
    }

    void ClickFavoriteButton()
    {
        IsFavorite = !IsFavorite;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
