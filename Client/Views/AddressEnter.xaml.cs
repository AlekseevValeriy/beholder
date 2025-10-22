namespace Beholder.Views;

public partial class AddressEnter : ContentPage
{
	public AddressEnter()
	{
		InitializeComponent();
		BindingContext = new AddressEnterViewModel();
	}
}