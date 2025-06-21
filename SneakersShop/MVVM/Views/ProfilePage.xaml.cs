using SneakersShop.MVVM.ViewModels;

namespace SneakersShop.MVVM.Views;

public partial class ProfilePage : ContentPage
{
	private readonly ProfileViewModel _viewModel;
	public ProfilePage()
	{
		InitializeComponent();
		_viewModel = new ProfileViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadUserCommand.ExecuteAsync(null);
    }

    private async void Edit_Profile_Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UpdateProfilePage(_viewModel.User));
    }

    private async void Addresses_Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddressesPage());
    }

    private async void Orders_Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new OrdersPage());
    }

    private async void Change_Password_Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChangePasswordPage());
    }

    private async void Contact_Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ContactPage());
    }

    private async void Privacy_Policy_Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PrivacyPolicyPage());
    }

    private void Logout_Button_Clicked(object sender, EventArgs e)
    {
        SecureStorage.Default.Remove("user");
        if(Application.Current != null)
            Application.Current.MainPage = new LoginPage();
    }

    private async void Login_Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    
}