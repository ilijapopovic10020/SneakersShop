namespace SneakersShop.MVVM.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

    private void Login_Button_Clicked(object sender, EventArgs e)
    {
        if (Application.Current != null)
            Application.Current.MainPage = new LoginPage();
    }

}