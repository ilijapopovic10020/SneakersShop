namespace SneakersShop.MVVM.Views;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

    private void Login_Button_Clicked(object sender, EventArgs e)
    {
        if (Application.Current != null)
            Application.Current.MainPage = new LoginPage();
    }

    private void Skip_Login_Button_Clicked(object sender, EventArgs e)
    {
        if (Application.Current != null)
            Application.Current.MainPage = new AppShell();
    }
}