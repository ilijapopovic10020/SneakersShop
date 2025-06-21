using CommunityToolkit.Maui.Core.Platform;
using SneakersShop.Controls;
using SneakersShop.Helpers;
using SneakersShop.MVVM.ViewModels;
using UraniumUI.Pages;

namespace SneakersShop.MVVM.Views;

public partial class ProductsPage : UraniumContentPage
{
    public ProductsViewModel ViewModel { get; }
    double _lastScrollY = 0;
    bool _isAnimating = false;
    public ProductsPage()
    {
        InitializeComponent();
        ViewModel = new ProductsViewModel();
        BindingContext = ViewModel;
        ViewModel.LoadFiltersCommand.ExecuteAsync(null);
        ViewModel.LoadProductsCommand.ExecuteAsync(null);
    }

    private async void Search_Button_Clicked(object sender, EventArgs e)
    {
        await ViewModel.SearchCommand.ExecuteAsync(null);

        DummyEntry.Focus();
        await DummyEntry.HideKeyboardAsync(default);
        DummyEntry.Unfocus();
    }

    private async void SearchField_Completed(object sender, EventArgs e)
    {
        await ViewModel.SearchCommand.ExecuteAsync(null);

        DummyEntry.Focus();
        await DummyEntry.HideKeyboardAsync(default);
        DummyEntry.Unfocus();
    }

    private void Filter_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        FiltersBottomSheet.IsPresented = true;
    }

    private async void ProductsScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (_isAnimating) return;

        if (e.ScrollY > 50 && ViewModel.IsFilterVisible)
        {
            _isAnimating = true;
            await FilterGrid.FadeTo(0, 300);
            await GoToTopButton.FadeTo(1, 300);
            ViewModel.IsFilterVisible = false;
            _isAnimating = false;
        }
        else if (e.ScrollY <= 10 && !ViewModel.IsFilterVisible)
        {
            _isAnimating = true;
            ViewModel.IsFilterVisible = true;
            await FilterGrid.FadeTo(1, 300);
            await GoToTopButton.FadeTo(0, 300);
            _isAnimating = false;
        }
    }

    private async void Go_To_Top_ImageButton_Clicked(object sender, EventArgs e)
    {
        if (ProductsScrollView.ScrollY > 0)
        {
            _lastScrollY = ProductsScrollView.ScrollY;
            await ProductsScrollView.ScrollToAsync(0, 0, true);
        }
    }
}