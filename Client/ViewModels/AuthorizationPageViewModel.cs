namespace Beholder.ViewModels;
public partial class AuthorizationPageViewModel : INotifyPropertyChanged
{
    readonly AppState _appState;
    readonly INavigationService _navigation;
    readonly IDataLoaderService _dataLoader;

    Boolean _loginIsValid = false;
    Boolean _passwordIsValid = false;
    ContentPage? _page;
    String _login = "";
    String _password = "";

    public Boolean LoginIsValid
    {
        get => _loginIsValid;
        set
        {
            if (value != _loginIsValid)
            {
                _loginIsValid = value;
                OnPropertyChanged();
            }
        }
    }
    public Boolean PasswordIsValid
    {
        get => _passwordIsValid;
        set
        {
            if (value != _passwordIsValid)
            {
                _passwordIsValid = value;
                OnPropertyChanged();
            }
        }
    }
    public String Login
    {
        get => _login;
        set
        {
            if (value != _login)
            {
                _login = value;
                OnPropertyChanged();

                LoginIsValid = LoginFormatValidate(Login);
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

                PasswordIsValid = PasswordFormatValidate(Password);
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

    async Task<Boolean> UserDataValidate()
    {
        if (PasswordIsValid && LoginIsValid) return true;

        if (_page is not null) await _page.DisplayAlert("Ошибка формата", "Был введён неверный формат данных. Пожалуйста, введите данные соответствующие формату.", "Ок");

        return false;
    }

    async void Authorization()
    {
        if (!await UserDataValidate()) return;

        await _appState.GetUserAsync(Login, Password);

        if (_appState.User is not null)
        {
            if (_appState.User.HasProblem)
            {
                if (_appState.User.HttpError is not null && _appState.User.HttpError is System.Net.HttpStatusCode.NotFound && _page is not null)
                {
                    await _page.DisplayAlert("Не найден", "Не удалось найти пользователя с таким логином. Пожалуйста, зарегистрируетесь.", "Ок");
                }
            }
            else if (_appState.User.IsSuccess)
            {
                AccountEnter();
            }
        }
    }

    async void Registration()
    {
        if (!await UserDataValidate()) return;

        await _appState.AddUserAsync(Login, Password);

        if (_appState.User is not null)
        {
            if (_appState.User.HasProblem)
            {
                if (_appState.User.HttpError is not null && _appState.User.HttpError is System.Net.HttpStatusCode.Conflict && _page is not null)
                {
                    await _page.DisplayAlert("Конфликт", "Такой пользователь уже существует. Пожалуйста, введите другой логин.", "Ок");
                }
            }
            else if (_appState.User.IsSuccess)
            {
                AccountEnter();
            }
        }

    }

    async void AccountEnter()
    {
        if (_appState.User is null || _appState.User.HasProblem || _appState.User.Content is null || _page is null) return;

        _dataLoader.Upload(_appState.User.Content.login, _appState.User.Content.password);

        await _appState.LoadFavoritesAsync(_appState.User.Content.id);

        await _navigation.NavigateToAccountPageAsync(_page);
    }

    Boolean LoginFormatValidate(String login)
    {
        return login.Length >= 3 && login.Length <= 16;
    }

    Boolean PasswordFormatValidate(String password)
    {
        return password.Length >= 8 && password.Length <= 20;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
