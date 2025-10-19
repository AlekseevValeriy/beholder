namespace Beholder.Controls;

public partial class ChannelImage : ContentView
{
    public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image), typeof(String), typeof(Channel), default(String));
    public String Image
    {
        get => (String)GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public ChannelImage()
	{
		InitializeComponent();
	}
}