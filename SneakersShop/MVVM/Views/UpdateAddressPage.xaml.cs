using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.ViewModels;

namespace SneakersShop.MVVM.Views;

public partial class UpdateAddressPage : ContentPage
{
	private UpdateAddressViewModel _viewModel;
	public UpdateAddressPage(AddressModel address)
	{
		InitializeComponent();
        _viewModel = new UpdateAddressViewModel();
        BindingContext = _viewModel;
		_viewModel.Address = address;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadCitiesCommand.ExecuteAsync(null);
    }

    private async void Back_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		await Navigation.PopAsync();
    }
}