namespace Beholder.ViewModels;

public partial class AccountPageViewModel : INotifyPropertyChanged
{
    readonly AppState _appState;
    readonly IDataLoaderService _dataLoader;

    Boolean _isPasswordVisiable = true;
    public UserCreateResponse? User
    {
        get
        {
            if (_appState.User is not null && _appState.User.IsSuccess && _appState.User.Content is not null)
                return _appState.User.Content;
            return null;
            
        }
    }
    ContentPage? _page;

    public Boolean IsPasswordVisiable
    {
        get => _isPasswordVisiable;
        set
        {
            if (value != _isPasswordVisiable)
            {
                _isPasswordVisiable = value;
                OnPropertyChanged();
            }
        }
    }
    public String Login
    {
        get
        {
            if (User is null) return "";
            return User.login;
        }
    }
    public String Password
    {
        get
        {
            if (User is null) return "";
            return User.password;
        }
    }

    public ICommand ToBackCommand { get; set; }
    public ICommand LogoutCommand { get; set; }
    public ICommand DeleteAccountCommand { get; set; }
    public ICommand OnEyeClickCommand { get; set; }

    public AccountPageViewModel(IDataLoaderService dataLoader, AppState appState)
    {
        _dataLoader = dataLoader;
        _appState = appState;
        _appState.PropertyChanged += OnAppStatePropertyChanged;

        ToBackCommand = new Command(ToBack);
        LogoutCommand = new Command(Logout);
        DeleteAccountCommand = new Command(DeleteAccount);
        OnEyeClickCommand = new Command(OnEyeClick);
    }

    void OnAppStatePropertyChanged(Object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AppState.User))
        {
            OnPropertyChanged(nameof(User));
        }
    }

    public AccountPageViewModel BindPage(ContentPage page)
    {
        _page = page;
        return this;
    }

    async void ToBack()
    {
        await Shell.Current.GoToAsync("//favorite");
    }

    void Logout()
    {
        _appState.ClearUser();

        if (_appState.User is null)
        {
            _dataLoader.Upload("", "");

            ToBack();
        }
    }

    async void DeleteAccount()
    {
        await _appState.DeleteUserAsync();

        if (_appState.User is null)
        {
            _dataLoader.Upload("", "");

            ToBack();
        }
    }

    void OnEyeClick()
    {
        IsPasswordVisiable = !IsPasswordVisiable;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
