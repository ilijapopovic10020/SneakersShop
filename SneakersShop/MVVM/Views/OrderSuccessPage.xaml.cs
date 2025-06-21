namespace SneakersShop.MVVM.Views;

public partial class OrderSuccessPage : ContentPage
{
	public OrderSuccessPage()
	{
		InitializeComponent();
	}

    private void Back_To_Home_Button_Clicked(object sender, EventArgs e)
    {
		if (Application.Current != null)
			Application.Current.MainPage = new AppShell();
    }
}