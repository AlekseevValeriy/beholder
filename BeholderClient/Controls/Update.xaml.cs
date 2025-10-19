using static System.Net.Mime.MediaTypeNames;

namespace Beholder.Controls;

public partial class Update : ContentView
{
    Int32 _grad = 180;

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Tag), default(ICommand));
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public Update()
	{
		InitializeComponent();
	}

    async void Rotate()
    {
        await UpdateIcon.RotateTo(_grad, 625, Easing.CubicInOut);

        _grad = (_grad + 180) % 360;
    }

    void ClickedCommand(Object sender, EventArgs e)
    {

        if (Command is not null && Command.CanExecute(null))
        {
            Command.Execute(null);
        }

        Rotate();
    }
}