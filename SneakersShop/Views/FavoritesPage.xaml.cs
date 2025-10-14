using CommunityToolkit.Maui.Core.Platform;
using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class FavoritesPage : ContentPage
{
	private readonly FavoritesViewModel _vm;
	public FavoritesPage(FavoritesViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadFavoritesCommand.ExecuteAsync(null);
    }

    private async void SearchField_Completed(object sender, EventArgs e)
    {
        await _vm.LoadFavoritesCommand.ExecuteAsync(null);

        DummyEntry.Focus();
        await DummyEntry.HideKeyboardAsync(default);
        DummyEntry.Unfocus();
    }
}