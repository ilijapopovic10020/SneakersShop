using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class ProductsPage : ContentPage
{
    private readonly ProductsViewModel _vm;
    public ProductsPage(ProductsViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        _vm = vm;

        _vm.LoadCategoriesCommand.ExecuteAsync(null);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadProductsCommand.ExecuteAsync(null);
    }
}