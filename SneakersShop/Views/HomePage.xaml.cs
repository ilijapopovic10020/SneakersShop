using SneakersShop.ViewModels;
using System.Threading.Tasks;

namespace SneakersShop.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _vm;
	public HomePage(HomeViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadRecommendedCommand.ExecuteAsync(null);
    }
}