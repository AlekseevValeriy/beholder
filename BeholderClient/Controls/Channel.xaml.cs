namespace Beholder.Controls;

public partial class Channel : ContentView
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

    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(String), typeof(Channel), default(String));
    public String Description
    {
        get => (String)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly BindableProperty NumberProperty = BindableProperty.Create(nameof(Number), typeof(Int32), typeof(Channel), default(Int32));
    public Int32 Number
    {
        get => (Int32)GetValue(NumberProperty);
        set => SetValue(NumberProperty, value);
    }

    public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image), typeof(String), typeof(Channel), default(String));
    public String Image
    {
        get => (String)GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public static readonly BindableProperty TagsProperty = BindableProperty.Create(nameof(Tags), typeof(String), typeof(Channel), default(String), propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
    {
        if (bindable is not Channel control || newValue is not String tags) return;

        foreach (String tag in tags.Split(","))
        {
            control.TagsLayout.Add(new Tag() { Text = tag });
        }
    });
    public String Tags
    {
        get => (String)GetValue(TagsProperty);
        set => SetValue(TagsProperty, value);
    }

    public static readonly BindableProperty TapCommandProperty = BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(Channel), default(ICommand));
    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    public Channel()
    {
        InitializeComponent();
    }

    private void OpenTeleprogramPage(Object sender, TappedEventArgs e)
    {
        TapCommand.Execute(ChannelId);
    }
}