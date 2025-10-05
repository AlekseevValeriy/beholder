namespace Beholder.Controls;

public partial class Filter : ContentView
{
    public FilterMS StateMachine { get; private set; } = new();
    public Double ImageSize { get; } = 15;

    public delegate void EventHandler(Object sender);
    public event EventHandler? BecameFocused;


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

    public Filter()
    {
        InitializeComponent();
        StateMachine.StateChanged += StateChange;
    }

    async void StateChange(FilterState state)
    {
        switch (state)
        {
            case FilterState.Idle:
                {
                    Background.BackgroundColor = Color.FromArgb("#E3F2FD");

                    TextLabel.TextColor = Color.FromArgb("#0D47A1");

                    ArrowImage.IsVisible = false;
                    ArrowImage.WidthRequest = 0;
                    await Task.WhenAll(
                        ArrowImage.LayoutTo(new Rect(0.0, 0.0, 0.0, ImageSize)),
                        ArrowImage.RotateTo(0));
                    break;
                }
            case FilterState.DescendingSort:
                {
                    BecameFocused?.Invoke(this);

                    Background.BackgroundColor = Color.FromArgb("#FFFFFF");

                    TextLabel.TextColor = Color.FromArgb("#424242");

                    ArrowImage.IsVisible = true;
                    ArrowImage.WidthRequest = ImageSize;
                    await Task.Run(() => ArrowImage.LayoutTo(new Rect(0.0, 0.0, ImageSize, ImageSize)));
                    break;
                }
            case FilterState.AscendingSort:
                {
                    await Task.Run(() => ArrowImage.RotateTo(-180, 400, Easing.CubicInOut));
                    break;
                }
            default:
                {
                    goto case FilterState.Idle;
                }
        }
    }

    void ClickedCommand(Object sender, EventArgs e)
    {
        StateMachine.NextState();

        FilterData parameter = new FilterData(Text, StateMachine.State);

        if (Command is not null && Command.CanExecute(parameter))
        {
            Command.Execute(parameter);
        }
    }
}

public class FilterMS
{
    FilterState _state = FilterState.Idle;

    public delegate void EventHandler(FilterState state);
    public event EventHandler? StateChanged;

    public FilterState State => _state;

    public void NextState()
    {
        _state = _state switch
        {
            FilterState.Idle => FilterState.DescendingSort,
            FilterState.DescendingSort => FilterState.AscendingSort,
            FilterState.AscendingSort or _ => FilterState.Idle
        };

        StateChanged?.Invoke(_state);
    }
    public void ToIdle()
    {
        _state = FilterState.Idle;

        StateChanged?.Invoke(_state);
    }
}

public enum FilterState
{
    Idle,
    DescendingSort,
    AscendingSort
}

public class FilterData(String text, FilterState state)
{
    public String Text { get; } = text;
    public FilterState State { get; } = state;

}