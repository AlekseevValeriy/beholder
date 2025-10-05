namespace Beholder.ViewModels;
public partial class AuthorizationPageViewModel : INotifyPropertyChanged
{
    readonly AppState _appState;
    readonly INavigationService _navigation;
    readonly IDataLoaderService _dataLoader;

    ContentPage? _page;
    String _login = "";
    String _password = "";

    public String Login
    {
        get => _login;
        set
        {
            if (value != _login)
            {
                _login = value;
                OnPropertyChanged();

                LoginValidate();
            }
        }
    }
    public String Password
    {
        get => _password;
        set
        {
            if (value != _password)
            {
                _password = value;
                OnPropertyChanged();

                PasswordValidate();
            }
        }
    }

    public ICommand ToBackCommand { get; set; }
    public ICommand AuthorizationCommand { get; set; }
    public ICommand RegistrationCommand { get; set; }

    public AuthorizationPageViewModel(IDataLoaderService dataLoader, INavigationService navigation, AppState appState)
    {
        _navigation = navigation;
        _dataLoader = dataLoader;
        _appState = appState;

        ToBackCommand = new Command(ToBack);
        AuthorizationCommand = new Command(Authorization);
        RegistrationCommand = new Command(Registration);
    }

    public AuthorizationPageViewModel BindPage(ContentPage page)
    {
        _page = page;
        return this;
    }

    async void ToBack()
    {
        await Shell.Current.GoToAsync("//favorite");
    }

    async void Authorization()
    {
        await _appState.GetUserAsync(Login, Password);

        if (_appState.User is null || _appState.User.HasProblem || _appState.User.Content is null || _page is null) return;

        _dataLoader.Upload(_appState.User.Content.login, _appState.User.Content.password);

        await _navigation.NavigateToAccountPageAsync(_page);
    }

    async void Registration()
    {
        await _appState.AddUserAsync(Login, Password);

        if (_appState.User is null || _appState.User.HasProblem || _appState.User.Content is null || _page is null) return;

        _dataLoader.Upload(_appState.User.Content.login, _appState.User.Content.password);

        await _navigation.NavigateToAccountPageAsync(_page);
    }

    void LoginValidate()
    {

    }

    void PasswordValidate()
    {

    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
