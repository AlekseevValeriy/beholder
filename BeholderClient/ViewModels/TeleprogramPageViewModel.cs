using System.Threading.Tasks;

namespace Beholder.ViewModels;

public partial class TeleprogramPageViewModel : INotifyPropertyChanged
{
    public readonly AppState appState;

    ContentPage? _page;
    Int32 _channelId = -1;
    Boolean _tagsContains = false;
    Boolean _schudleContains = false;
    Boolean _isFavorite = false;
    String _pastButtonText = "";
    DateTime _pastDate = DateTime.Today.Date;
    String _futureButtonText = "";
    DateTime _futureDate = DateTime.Today.Date;
    ObservableCollection<ScheduleResponseGroup>? _groupedSchedules;
    Boolean _isAuthorized = false;
    Boolean _isLoadSucces = false;
    Boolean _isBusy = false;
    Boolean _isFutureBusy = false;
    Boolean _isPastBusy = false;
    Boolean _isFutureVisiable = false;
    Boolean _isPastVisiable = false;
    Problem _problemContent = Problem.None;
    ChannelResponse? _channel;
    ObservableCollection<ScheduleResponse>? _schedule;

    public Boolean IsPastVisiable
    {
        get => _isPastVisiable;
        set
        {
            if (value != _isPastVisiable)
            {
                _isPastVisiable = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean IsFutureVisiable
    {
        get => _isFutureVisiable;
        set
        {
            if (value != _isFutureVisiable)
            {
                _isFutureVisiable = value;
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
    public Boolean IsPastBusy
    {
        get => _isPastBusy;
        set
        {
            if (value != _isPastBusy)
            {
                _isPastBusy = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean IsFutureBusy
    {
        get => _isFutureBusy;
        set
        {
            if (value != _isFutureBusy)
            {
                _isFutureBusy = value;
                OnPropertyChanged();
            }
        }
    }
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
        get => _channel;
        set
        {
            if (value != _channel)
            {
                _channel = value;
                OnPropertyChanged();
            }
        }
    }
    
    public ObservableCollection<ScheduleResponse>? Schedule
    {
        get => _schedule;
        set
        {
            if (value != _schedule)
            {
                _schedule = value;
                OnPropertyChanged();
            }
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
    }

    public async void LoadData(Int32 channelId)
    {
        _channelId = channelId;

        {
            IsLoadSucces = true;
            IsBusy = true;

            var response = await appState.LoadChannelAsync(_channelId);

            if (response is not null)
            {
                if (response.HasProblem)
                {
                    ProblemContent = ProblemHandleHelper.ProblemHandle<ChannelResponse>(response);
                    IsLoadSucces = false;
                }
                else if (response.Content is not null)
                {
                    Channel = response.Content;
                }
            }

            IsBusy = false;

            TagsContaing = Channel?.tags is not null && Channel.tags.Length > 0;
            IsLoadSucces = true;
        }

        {
            if (appState.User is not null && appState.User.IsSuccess && appState.User.Content is not null)
            {
                IsAuthorized = true;

                var result = await appState.hasFavoriteAsync(appState.User.Content.id, _channelId);
                if (result.IsSuccess) IsFavorite = result.Content;
            }
        }

        {
            AddSchudle(DateTime.Today, ScheduleLoadPosition.Just);
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
        PastButtonText = $"Программы на {_pastDate:dd.MM.yyyy}";
    }

    void MoveNextFutureText()
    {
        _futureDate = _futureDate.AddDays(1);
        FutureButtonText = $"Программы на {_futureDate:dd.MM.yyyy}";
    }

    async void PastButtonClick()
    {
        AddSchudle(_pastDate, ScheduleLoadPosition.Past);

        MoveNextPastText();

        if (!await IsDateActual(_pastDate))
        {
            IsPastVisiable = false;
        }
    }
    async void FutureButtonClick()
    {
        AddSchudle(_futureDate, ScheduleLoadPosition.Future);

        MoveNextFutureText();

        if (!await IsDateActual(_futureDate))
        {
            IsFutureVisiable = false;
        }
    }

    async Task<Boolean> IsDateActual(DateTime date)
    {
        ApiResponse<Boolean> response = await appState.HasScheduleAsync(_channelId, date);

        if (response.IsSuccess) return response.Content;
        return false;
    }

    async void AddSchudle(DateTime date, ScheduleLoadPosition postion)
    {
        switch (postion)
        {
            case ScheduleLoadPosition.Just | ScheduleLoadPosition.Past: IsPastBusy = true; break;
            default: IsFutureBusy = true; break;
        }

        var response = await appState.LoadScheduleAsync(_channelId, date);

        if (response is not null && response.IsSuccess && response.Content is not null && response.Content.Count > 0)
        {
            if (postion == ScheduleLoadPosition.Just)
            {
                _futureDate = DateTime.Today;
                MoveNextFutureText();
                if (await IsDateActual(_futureDate))
                {
                    IsFutureVisiable = true;
                }

                _pastDate = DateTime.Today;
                MoveNextPastText();
                if (await IsDateActual(_pastDate))
                {
                    IsPastVisiable = true;
                }

                SchudleContains = true;
            }

            if (GroupedSchedules is null)
            {
                GroupedSchedules = new(response.Content
                    .OrderBy(x => x.start_time)
                    .GroupBy(s => $"{s.start_time:dd.MM.yyyy}")
                    .Select(g => new ScheduleResponseGroup(g.Key, new(g))));
            }
            else
            {
                GroupedSchedules = new(GroupedSchedules
                    .SelectMany(g => g)
                    .ToList()
                    .Concat(response.Content)
                    .OrderBy(x => x.start_time)
                    .GroupBy(s => $"{s.start_time:dd.MM.yyyy}")
                    .Select(g => new ScheduleResponseGroup(g.Key, new(g))));
            }

            IsPastBusy = false;
            IsFutureBusy = false;
        }
    }

    async void ClickFavoriteButton()
    {
        if (_channelId != -1 && appState.User is not null && appState.User.IsSuccess && appState.User.Content is not null)
        {
            switch (IsFavorite)
            {
                case true:
                    {
                        var response = await appState.DeletefavoriteAsync(_channelId, appState.User.Content.id);
                        if (response.IsSuccess && response.Content == true) IsFavorite = false;
                        break;
                    }
                case false:
                    {
                        var response = await appState.AddfavoriteAsync(_channelId, appState.User.Content.id);
                        if (response.IsSuccess && response.Content == true) IsFavorite = true;
                        break;
                    }
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
