using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class AddressesPage : ContentPage
{
    private readonly AddressesViewModel _vm;
	public AddressesPage(AddressesViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadAddressesCommand.ExecuteAsync(null);
    }

    private async void Add_Address_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddressCreatePage));
    }
}