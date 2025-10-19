namespace Beholder.ViewModels;

internal class AddressEnterViewModel : INotifyPropertyChanged
{
    String _address = "https://certainly-peaceful-rabbit.cloudpub.ru:443";

    public String Address
    {
        get => _address;
        set
        {
            if (value != _address)
            {
                _address = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand ToShellCommand { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public AddressEnterViewModel()
    {
        ToShellCommand = new Command(ToShell);
    }

    public void OnPropertyChanged([CallerMemberName] String prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    void ToShell()
    {
        if (Address != "" && Address.Contains(@"https://")) ApiClient.Address = Address;

        if (Application.Current?.Windows[0] is Window window)
        {
            window.Page = new AppShell();
        }
    }
}
