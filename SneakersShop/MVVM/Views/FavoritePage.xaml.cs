using CommunityToolkit.Maui.Core.Platform;
using SneakersShop.MVVM.ViewModels;

namespace SneakersShop.MVVM.Views;

public partial class FavoritePage : ContentPage
{
	public FavoriteViewModel ViewModel { get; }
    double _lastScrollY = 0;
    bool _isAnimating = false;
    public FavoritePage()
	{
		InitializeComponent();
        ViewModel = new FavoriteViewModel();
        BindingContext = ViewModel;
        ViewModel.LoadFavoritesCommand.ExecuteAsync(null);
    }

    private async void Search_Button_Clicked(object sender, EventArgs e)
    {
        await ViewModel.LoadFavoritesCommand.ExecuteAsync(null);

        DummyEntry.Focus();
        await DummyEntry.HideKeyboardAsync(default);
        DummyEntry.Unfocus();
    }

    private async void SearchField_Completed(object sender, EventArgs e)
    {
        await ViewModel.LoadFavoritesCommand.ExecuteAsync(null);

        DummyEntry.Focus();
        await DummyEntry.HideKeyboardAsync(default);
        DummyEntry.Unfocus();
    }

    private async void FavoritesScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (_isAnimating) return;

        if (e.ScrollY > 50)
        {
            _isAnimating = true;
            await GoToTopButton.FadeTo(0, 300);
            ViewModel.IsScrolled = true;
            _isAnimating = false;
        }
        else if (e.ScrollY <= 10)
        {
            _isAnimating = true;
            ViewModel.IsScrolled = false;
            await GoToTopButton.FadeTo(1, 300);
            _isAnimating = false;
        }
    }

    private async void Go_To_Top_ImageButton_Clicked(object sender, EventArgs e)
    {
        if (FavoritesScrollView.ScrollY > 0)
        {
            _lastScrollY = FavoritesScrollView.ScrollY;
            await FavoritesScrollView.ScrollToAsync(0, 0, true);
        }
    }
}