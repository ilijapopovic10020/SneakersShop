using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.ViewModels;

namespace SneakersShop.MVVM.Views;

public partial class CreateOrderPage : ContentPage
{
    private readonly CreateOrderViewModel _viewModel;
    public CreateOrderPage(List<CreateOrderItem> orderItems, double? totalPrice, int cartQuantity, CartViewModel cartViewModel)
	{
		InitializeComponent();
        _viewModel = new();
        BindingContext = _viewModel;
        _viewModel.CartViewModel = cartViewModel;

        _viewModel.OrderItems = orderItems;
        _viewModel.TotalPrice = $"{totalPrice},00 RSD";
        _viewModel.CartQuantity = $"Proizvodi ({cartQuantity} artikla)";
    }

    private async void Back_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		await Navigation.PopAsync();
    }

    private async void AddAddress_Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync(nameof(CreateAddressPage));
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadAddressesCommand.ExecuteAsync(null);
    }
}