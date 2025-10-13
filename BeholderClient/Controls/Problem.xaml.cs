namespace Beholder.Controls;

public partial class Problem : ContentView
{
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(String), typeof(Problem), default(String));
    public String ImageSource
    {
        get => (String)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(String), typeof(Problem), default(String));
    public String Text
    {
        get => (String)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Problem()
	{
		InitializeComponent();
	}

    public static Problem ApiError => new() { Text= "В программе произошла непредвиденная ошибка", ImageSource= "problem.png" };
    public static Problem InternalError => new() { Text= "На сервере произошла непредвиденная ошибка", ImageSource= "db_internal_error.png" };
    public static Problem NoConnect => new() { Text= "Осутствует подключение к серверу", ImageSource= "db_no_connect.png" };
    public static Problem NotFound => new() { Text= "Не удалось найти информацию", ImageSource= "not_found.png" };
    public static Problem None => new() { Text= "", ImageSource= "" };
}