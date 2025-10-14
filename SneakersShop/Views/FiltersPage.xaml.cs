using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class FiltersPage : ContentPage
{
    private readonly FiltersViewModel _vm;
	public FiltersPage(FiltersViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadFiltersCommand.ExecuteAsync(null);
    }
}