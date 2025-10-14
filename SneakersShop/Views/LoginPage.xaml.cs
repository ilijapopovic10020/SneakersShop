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
}