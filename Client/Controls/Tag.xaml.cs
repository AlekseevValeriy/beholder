namespace Beholder.Controls;

public partial class Tag : ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(String), typeof(Tag), default(String));
    public String Text
    {
        get => (String)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Tag), default(ICommand));
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public Tag()
    {
        InitializeComponent();
    }

    void ClickedCommand(Object sender, EventArgs e)
    {
        if (Command is not null && Command.CanExecute(e))
        {
            Command.Execute(e);
        }
    }
}