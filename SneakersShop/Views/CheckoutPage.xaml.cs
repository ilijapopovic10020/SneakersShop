using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class CheckoutPage : ContentPage
{
	private readonly CheckoutViewModel _vm;
	public CheckoutPage(CheckoutViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
        BindingContext = _vm;
    }

    private async void AddAddress_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddressCreatePage));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadAddressesCommand.ExecuteAsync(null);
    }
}