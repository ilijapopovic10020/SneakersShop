using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SneakersShop.MVVM.Views;

public partial class AddressesPage : ContentPage
{
    private readonly AddressesViewModel _viewModel;
	public AddressesPage()
	{
		InitializeComponent();
        _viewModel = new AddressesViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadAddressesCommand.ExecuteAsync(null);
    }

    private async void Back_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void Add_Address_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new CreateAddressPage());
    }

    private async void Edit_Address_SwipeItem_Clicked(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is AddressModel address)
        {
            await Navigation.PushAsync(new UpdateAddressPage(address));
        }
    }
}