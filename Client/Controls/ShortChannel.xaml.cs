namespace Beholder.Controls;

public partial class ShortChannel : ContentView
{
    public static readonly BindableProperty ChannelIdProperty = BindableProperty.Create(nameof(ChannelId), typeof(Int32), typeof(Channel), default(Int32));
    public Int32 ChannelId
    {
        get => (Int32)GetValue(ChannelIdProperty);
        set => SetValue(ChannelIdProperty, value);
    }

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(String), typeof(Channel), default(String));
    public String Title
    {
        get => (String)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image), typeof(String), typeof(Channel), default(String));
    public String Image
    {
        get => (String)GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public static readonly BindableProperty TapCommandProperty = BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(Channel), default(ICommand));
    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    public ShortChannel()
    {
        InitializeComponent();
    }

    private void OpenTeleprogramPage(Object sender, TappedEventArgs e)
    {
        TapCommand.Execute(ChannelId);
    }
}