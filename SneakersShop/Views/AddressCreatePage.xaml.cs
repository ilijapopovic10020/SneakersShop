using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class AddressCreatePage : ContentPage
{
    private readonly AddressCreateViewModel _vm;
    public AddressCreatePage(AddressCreateViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadCitiesCommand.ExecuteAsync(null);
    }
}