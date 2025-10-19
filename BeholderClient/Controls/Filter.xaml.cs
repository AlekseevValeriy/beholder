using Beholder.Extensions;

namespace Beholder.Controls;

public partial class Filter : ContentView
{
    static readonly Color _defaultColor = Color.FromArgb("#cd0aa5");

    static readonly Color BackroundColorIdle = Application.Current?.RequestedTheme switch
    {
        AppTheme.Light => App.Current.Resources.GetColorOrDefault("SecondaryLight", _defaultColor),
        AppTheme.Dark => App.Current.Resources.GetColorOrDefault("SecondaryDark", _defaultColor),
        _ => Colors.Transparent
    };
    static readonly Color BackgroundColorSort = Application.Current?.RequestedTheme switch
    { 
        AppTheme.Light => App.Current.Resources.GetColorOrDefault("FilterBackgroundSortLight", _defaultColor),
        AppTheme.Dark => App.Current.Resources.GetColorOrDefault("FilterBackgroundSortDark", _defaultColor),
        _ => Colors.Transparent
    };
    static readonly Color TextColorIdle = Application.Current?.RequestedTheme switch
    { 
        AppTheme.Light => App.Current.Resources.GetColorOrDefault("FilterTextIdleLight", _defaultColor),
        AppTheme.Dark => App.Current.Resources.GetColorOrDefault("FilterTextIdleDark", _defaultColor),
        _ => Colors.Transparent
    };
    static readonly Color TextColorSort = Application.Current?.RequestedTheme switch
    { 
        AppTheme.Light => App.Current.Resources.GetColorOrDefault("FilterTextSortLight", _defaultColor),
        AppTheme.Dark => App.Current.Resources.GetColorOrDefault("FilterTextSortDark", _defaultColor),
        _ => Colors.Transparent
    };

    public FilterControlStateMachine StateMachine { get; private set; } = new();
    public Double ImageSize { get; } = 16;

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
                    if (Application.Current?.RequestedTheme is AppTheme.Dark)
                    {
                        Background.Stroke = BackroundColorIdle;
                        Background.HeightRequest = 30;
                    }
                    else
                    {
                        Background.BackgroundColor = BackroundColorIdle;
                        TextLabel.TextColor = TextColorIdle;
                    }


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

                    if (Application.Current?.RequestedTheme is AppTheme.Dark)
                    {
                        Background.Stroke = BackgroundColorSort;
                        Background.HeightRequest = 34;
                    }
                    else
                    {
                        Background.BackgroundColor = BackgroundColorSort;
                        TextLabel.TextColor = TextColorSort;
                    }

                    ArrowImage.IsVisible = true;
                    ArrowImage.WidthRequest = ImageSize;

                    await ArrowImage.LayoutTo(new Rect(0.0, 0.0, ImageSize, ImageSize));
                    break;
                }
            case FilterState.AscendingSort:
                {
                    await ArrowImage.RotateTo(-180, 400, Easing.CubicInOut);
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

public class FilterControlStateMachine
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

    public void Deconstruct(out FilterState state, out String text)
    {
        state = this.State;
        text = this.Text;
    }
}