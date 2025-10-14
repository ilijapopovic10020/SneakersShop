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
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadProductsCommand.ExecuteAsync(null);
    }

    private async void Filters_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FiltersPage));
    }
}