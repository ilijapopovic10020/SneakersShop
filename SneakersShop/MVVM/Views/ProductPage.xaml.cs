using Microsoft.VisualBasic;
using SneakersShop.MVVM.ViewModels;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SneakersShop.MVVM.Views;

public partial class ProductPage : ContentPage
{
    private int _productId;
    public ProductViewModel ViewModel;
    public ProductPage(int productId)
    {
        InitializeComponent();
        _productId = productId;
        ViewModel = new ProductViewModel();
        BindingContext = ViewModel;
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        ViewModel.LoadProductCommand.ExecuteAsync(_productId);
    }

    private async void Back_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.CurrentImage))
        {
            await FadeImageAsync(CurrentImageView);
        }
        else if (e.PropertyName == nameof(ViewModel.PreviousImage))
        {
            await FadeImageAsync(PreviousImageView);
        }
        else if (e.PropertyName == nameof(ViewModel.NextImage))
        {
            await FadeImageAsync(NextImageView);
        }
    }

    private async Task FadeImageAsync(Image image)
    {
        if (image == null)
            return;

        await image.FadeTo(0, 150, Easing.CubicOut);
        await Task.Delay(50); 
        await image.FadeTo(1, 250, Easing.CubicIn);
    }

    private async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (e.ScrollY > 100)
        {
            await initialAddToCart.FadeTo(0, 200);
            initialAddToCart.IsVisible = false;
            scrolledAddToCart.IsVisible = true;
            await scrolledAddToCart.FadeTo(1, 200);
        }
        else
        {
            await scrolledAddToCart.FadeTo(0, 200);
            scrolledAddToCart.IsVisible = false;
            initialAddToCart.IsVisible = true;
            await initialAddToCart.FadeTo(1, 200);
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

            await AnimateWidth(scrolledAddToCart, 60, 330, 250);

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

            await AnimateWidth(scrolledAddToCart, 330, 60, 250);

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

    private async void Reviews_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ReviewsPage(ViewModel.Product.Id));
    }

    private async void Size_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Border border)
        {
            await border.ScaleTo(0.9, 100, Easing.CubicOut);

            if (BindingContext is ProductViewModel vm && e.Parameter is int sizeId)
            {
                if (vm.SelectSizeCommand.CanExecute(sizeId))
                    vm.SelectSizeCommand.Execute(sizeId);
            }

            await border.ScaleTo(1, 100, Easing.CubicIn);
        }
    }
}