using SneakersShop.ViewModels;
using System.Threading.Tasks;

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

    private void Checkout_Button_Clicked(object sender, EventArgs e)
    {

    }
}