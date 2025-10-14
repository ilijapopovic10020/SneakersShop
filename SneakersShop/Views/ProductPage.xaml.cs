using Microsoft.Maui.Controls;
using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class ProductPage : ContentPage
{
    private readonly ProductViewModel _vm;
	public ProductPage(ProductViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

       await _vm.LoadProductCommand.ExecuteAsync(null);
    }

    private async void Size_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Border border)
        {
            await border.ScaleTo(0.9, 100, Easing.CubicOut);
            await border.ScaleTo(1, 100, Easing.CubicIn);
        }
    }

    private async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (e.ScrollY > 100)
        {
            await InitialAddToCart.FadeTo(0, 200);
            InitialAddToCart.IsVisible = false;
            ScrolledAddToCart.IsVisible = true;
            await ScrolledAddToCart.FadeTo(1, 200);
        }
        else
        {
            await ScrolledAddToCart.FadeTo(0, 200);
            ScrolledAddToCart.IsVisible = false;
            InitialAddToCart.IsVisible = true;
            await InitialAddToCart.FadeTo(1, 200);
        }

    }

    private async void Show_Cart_Options_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var clicked = addToCart.IsVisible;

        if (!clicked)
        {
            addToCart.IsVisible = false;
            buyNow.IsVisible = false;

            addToCartGrid.ColumnDefinitions.Clear();
            addToCartGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 125 });
            addToCartGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 125 });
            addToCartGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });

            await AnimateWidth(ScrolledAddToCart, 60, 330, 250);

            addToCart.Opacity = 0;
            buyNow.Opacity = 0;
            addToCart.IsVisible = true;
            buyNow.IsVisible = true;

            await Task.WhenAll(
                addToCart.FadeTo(1, 200),
                buyNow.FadeTo(1, 200)
            );
        }
        else
        {
            await Task.WhenAll(
                addToCart.FadeTo(0, 150),
                buyNow.FadeTo(0, 150)
            );

            addToCart.IsVisible = false;
            buyNow.IsVisible = false;

            await AnimateWidth(ScrolledAddToCart, 330, 60, 250);

            addToCartGrid.ColumnDefinitions.Clear();
        }
    }

    private Task AnimateWidth(VisualElement view, double from, double to, uint length)
    {
        var tcs = new TaskCompletionSource<bool>();

        var animation = new Animation(v =>
        {
            view.WidthRequest = v;
        }, from, to);

        animation.Commit(view, "WidthAnimation", 16, length, Easing.CubicInOut, (v, c) => tcs.SetResult(true));

        return tcs.Task;
    }
}