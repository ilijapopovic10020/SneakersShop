namespace SneakersShop.Views;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

    private async void Login_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    }

    private async void Skip_Login_Button_Clicked(object sender, EventArgs e)
    {
        if(Application.Current != null)
            Application.Current.MainPage = new AppShell();

        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }
}