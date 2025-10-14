using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class AddressUpdatePage : ContentPage
{
    private readonly AddressUpdateViewModel _vm;
	public AddressUpdatePage(AddressUpdateViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadDataCommand.ExecuteAsync(null);
    }
}