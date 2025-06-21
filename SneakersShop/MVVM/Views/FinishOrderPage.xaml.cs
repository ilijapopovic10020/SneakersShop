using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.ViewModels;
using SneakersShop.Services;

namespace SneakersShop.MVVM.Views;

public partial class FinishOrderPage : ContentPage
{
	private readonly List<CreateOrderItem> _orderItems;
    private readonly OrderService _orderService = new();
    private readonly CartViewModel _cartViewmodel;
    public FinishOrderPage(List<CreateOrderItem> orderItems, double? totalPrice, int cartQuantity, CartViewModel cartViewModel)
	{
		InitializeComponent();
		_orderItems = orderItems;
        _cartViewmodel = cartViewModel;

		((CreateOrderViewModel)BindingContext).OrderItems = orderItems;
		((CreateOrderViewModel)BindingContext).TotalPrice = $"{totalPrice},00 RSD";
		((CreateOrderViewModel)BindingContext).CartQuantity = $"Proizvodi ({cartQuantity} artikla)";
		
	}

    private async void Back_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		await Navigation.PopAsync();
    }

    private async void AddAddress_Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync(nameof(CreateAddressPage));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ((CreateOrderViewModel)BindingContext).LoadUserCommand.ExecuteAsync(null);
    }

    private async void Create_Order_Button_Clicked(object sender, EventArgs e)
    {
        var vm = ((CreateOrderViewModel)BindingContext);

        try
        {
            var order = new CreateOrderModel
            {
                PaymentType = (int)vm.SelectedPaymentType,
                Notes = vm.Notes,
                CardHolder = vm.CardHolder,
                CardNumber = vm.CardNumber,
                Cvv = vm.Cvv,
                Expiration = vm.Expiration,
                AddressId = vm.SelectedAddress.Id,
                Items = vm.OrderItems.Select(oi => new CreateOrderItem
                {
                    ProductColorId = oi.ProductColorId,
                    SizeId = oi.SizeId,
                    Quantity = oi.Quantity
                })
            };

            var result = await _orderService.Post(order);

            if (result)
            {
                _cartViewmodel.RemovePurchasedItems();

                await Navigation.PushAsync(new OrderSuccessPage());
            }
            else
            {
                await SnackbarHelper.ShowMessage("Greška prilikom poručivanja.");
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
}