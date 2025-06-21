using SneakersShop.MVVM.ViewModels;

namespace SneakersShop.MVVM.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
    }

    private void Register_Button_Clicked(object sender, EventArgs e)
    {
        if (Application.Current != null)
            Application.Current.MainPage = new RegisterPage();
    }
}