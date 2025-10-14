namespace SneakersShop.Views;

public partial class OrderSuccessPage : ContentPage
{
	public OrderSuccessPage()
	{
		InitializeComponent();
	}

    private async void Back_To_Home_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("../../../HomePage");
    }
}