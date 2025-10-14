using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class OrdersPage : ContentPage
{
	private readonly OrdersViewModel _vm;
    public OrdersPage(OrdersViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadOrdersCommand.ExecuteAsync(null);
    }
}