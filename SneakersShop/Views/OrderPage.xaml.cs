using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class OrderPage : ContentPage
{
	private readonly OrderViewModel _vm;
    public OrderPage(OrderViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadOrderCommand.ExecuteAsync(null);
    }
}