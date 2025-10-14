using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _vm;
	public ProfilePage(ProfileViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
       
        await _vm.LoadUserCommand.ExecuteAsync(null);
    }

    private async void Update_User_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfileUpdatePage));
    }

    private async void Addresses_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddressesPage));
    }

    private async void Orders_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(OrdersPage));
    }

    private async void Change_Password_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PasswordUpdatePage));
    }

    private async void Contact_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ContactPage));
    }

    private async void Privacy_Policy_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PolicyAndPrivacyPage));
    }

    private async void Logout_Button_Clicked(object sender, EventArgs e)
    {
        SecureStorage.Default.Remove("user");
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    }

    private async void Login_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }

}