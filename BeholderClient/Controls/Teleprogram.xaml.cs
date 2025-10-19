namespace Beholder.Controls;

public partial class Teleprogram : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(String), typeof(Teleprogram), default(String));
    public String Title
    {
        get => (String)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(String), typeof(Teleprogram), default(String));
    public String Description
    {
        get => (String)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly BindableProperty CategoryProperty = BindableProperty.Create(nameof(Category), typeof(String), typeof(Teleprogram), default(String));
    public String Category
    {
        get => (String)GetValue(CategoryProperty);
        set => SetValue(CategoryProperty, value);
    }

    public static readonly BindableProperty AgeRatingProperty = BindableProperty.Create(nameof(AgeRating), typeof(String), typeof(Teleprogram), default(String));
    public String AgeRating
    {
        get => (String)GetValue(AgeRatingProperty);
        set => SetValue(AgeRatingProperty, value);
    }

    public static readonly BindableProperty TimeStartProperty = BindableProperty.Create(nameof(TimeStart), typeof(DateTime), typeof(Teleprogram), default(DateTime));
    public DateTime TimeStart
    {
        get => (DateTime)GetValue(TimeStartProperty);
        set => SetValue(TimeStartProperty, value);
    }

    public static readonly BindableProperty TimeEndProperty = BindableProperty.Create(nameof(TimeEnd), typeof(DateTime), typeof(Teleprogram), default(DateTime));
    public DateTime TimeEnd
    {
        get => (DateTime)GetValue(TimeEndProperty);
        set => SetValue(TimeEndProperty, value);
    }

    public String Time => $"{TimeStart:hh:mm} - {TimeEnd:hh:mm}";

    public Teleprogram()
    {
        InitializeComponent();
    }
}