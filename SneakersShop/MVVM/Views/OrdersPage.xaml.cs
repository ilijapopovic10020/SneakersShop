using SneakersShop.MVVM.ViewModels;
using SneakersShop.Services;
using UraniumUI.Pages;

namespace SneakersShop.MVVM.Views;

public partial class OrdersPage : UraniumContentPage
{
    private readonly OrdersViewModel _viewModel;
    private readonly OrderService _orderService;
    public OrdersPage()
	{
		InitializeComponent();
        _viewModel = new();
        _orderService = new();

        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadOrdersCommand.ExecuteAsync(null);
    }

    private async void Back_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        OrderDetails.IsPresented = true;
    }
}