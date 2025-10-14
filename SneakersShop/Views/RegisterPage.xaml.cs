using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private async void Login_Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    }
}