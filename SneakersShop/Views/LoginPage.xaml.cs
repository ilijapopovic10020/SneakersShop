using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(AuthViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    private async void Register_Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    private async void Skip_Button_Clicked(object sender, EventArgs e)
    {
        if (Application.Current != null)
            Application.Current.MainPage = new AppShell();

        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }

    private async void Privacy_Policy_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PolicyAndPrivacyPage));
    }
}