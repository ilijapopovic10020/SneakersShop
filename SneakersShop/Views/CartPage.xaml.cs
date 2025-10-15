using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class CartPage : ContentPage
{
	private readonly CartViewModel _vm;
    public CartPage(CartViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadCartItemsCommand.ExecuteAsync(null);
    }

    private async void Login_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    }
}