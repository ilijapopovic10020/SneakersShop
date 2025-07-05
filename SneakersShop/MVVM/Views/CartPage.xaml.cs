using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.ViewModels;

namespace SneakersShop.MVVM.Views;

public partial class CartPage : ContentPage
{
    private CartViewModel _viewModel;
	public CartPage()
	{
		InitializeComponent();
        _viewModel = new CartViewModel();
        BindingContext = _viewModel;
        
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadCartItemsCommand.Execute(null);
    }

    private async void Checkout_Button_Clicked(object sender, EventArgs e)
    {
        var user = SecureStorage.Default.GetUser();
        if (user != null)
        {
            var itemsToOrder = ((CartViewModel)BindingContext).CartItems
                            .Where(x => x.IsSelected)
                            .Select(x => new CreateOrderItem
                            {
                                ProductColorId = x.Product.Id,
                                SizeId = x.Size.Id,
                                Quantity = x.Quantity
                            }).ToList();
            var totalPrice = ((CartViewModel)BindingContext).TotalPrice;
            var cartQuantity = ((CartViewModel)BindingContext).CartQuantitySelectedItems;

            if (itemsToOrder.Count == 0)
            {
                await SnackbarHelper.ShowMessage("Molimo selektujete bar jedan proizvod za porudžbinu.");
                return;
            }
            else
            {
                var cartViewModel = ((CartViewModel)BindingContext);
                await Navigation.PushAsync(new CreateOrderPage(itemsToOrder, totalPrice, cartQuantity, cartViewModel));
            }
        }
    }
}