using SneakersShop.Helpers;
using SneakersShop.MVVM.Views;
using SneakersShop.Services;

namespace SneakersShop.Components;

public partial class ProductVerticalVariantComponent : ContentView
{
    private readonly ProductService _productService = new();

    public static readonly BindableProperty ImageProperty =
       BindableProperty.Create(nameof(Image), typeof(string), typeof(ProductVerticalVariantComponent), "");

    public static readonly BindableProperty BrandProperty =
        BindableProperty.Create(nameof(Brand), typeof(string), typeof(ProductVerticalVariantComponent), "");

    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(ProductVerticalVariantComponent), "");

    public static readonly BindableProperty OldPriceProperty =
        BindableProperty.Create(nameof(OldPrice), typeof(double), typeof(ProductHorizontalVariantComponent), 0.0);

    public static readonly BindableProperty NewPriceProperty =
        BindableProperty.Create(nameof(NewPrice), typeof(double), typeof(ProductHorizontalVariantComponent), 0.0);

    public static readonly BindableProperty HasDiscountProperty =
        BindableProperty.Create(nameof(HasDiscount), typeof(bool), typeof(ProductHorizontalVariantComponent), false);

    public static readonly BindableProperty AvgRatingProperty =
        BindableProperty.Create(nameof(AvgRating), typeof(decimal), typeof(ProductVerticalVariantComponent), 0m);

    public static readonly BindableProperty CountReviewProperty =
        BindableProperty.Create(nameof(CountReview), typeof(int), typeof(ProductVerticalVariantComponent), 0);

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ProductVerticalVariantComponent), null);

    public static readonly BindableProperty DiscountTypeProperty =
        BindableProperty.Create(nameof(DiscountType), typeof(string), typeof(ProductVerticalVariantComponent), "");

    public static readonly BindableProperty HasDiscountTypeProperty =
        BindableProperty.Create(nameof(HasDiscountType), typeof(bool), typeof(ProductVerticalVariantComponent), false);

    public static readonly BindableProperty HasDiscountValueProperty =
       BindableProperty.Create(nameof(HasDiscountValue), typeof(bool), typeof(ProductVerticalVariantComponent), false);

    public static readonly BindableProperty DiscountValueProperty =
        BindableProperty.Create(nameof(DiscountValue), typeof(double), typeof(ProductHorizontalVariantComponent), 0.0);

    public static readonly BindableProperty ProductIdProperty =
        BindableProperty.Create(nameof(ProductId), typeof(int), typeof(ProductVerticalVariantComponent), 0);
    
    public string Image
    {
        get => (string)GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }
    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }
    public string Brand
    {
        get => (string)GetValue(BrandProperty);
        set => SetValue(BrandProperty, value);
    }
    public double OldPrice
    {
        get => (double)GetValue(OldPriceProperty);
        set => SetValue(OldPriceProperty, value);
    }

    public double NewPrice
    {
        get => (double)GetValue(NewPriceProperty);
        set => SetValue(NewPriceProperty, value);
    }

    public bool HasDiscount
    {
        get => (bool)GetValue(HasDiscountProperty);
        set => SetValue(HasDiscountProperty, value);
    }

    public bool HasDiscountType
    {
        get => (bool)GetValue(HasDiscountTypeProperty);
        set => SetValue(HasDiscountTypeProperty, value);
    }
    public bool HasDiscountValue
    {
        get => (bool)GetValue(HasDiscountValueProperty);
        set => SetValue(HasDiscountValueProperty, value);
    }

    public decimal AvgRating
    {
        get => (decimal)GetValue(AvgRatingProperty);
        set => SetValue(AvgRatingProperty, value);
    }
    public int CountReview
    {
        get => (int)GetValue(CountReviewProperty);
        set => SetValue(CountReviewProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public string DiscountType
    {
        get => (string)GetValue(DiscountTypeProperty);
        set => SetValue(DiscountTypeProperty, value);
    }

    public double DiscountValue
    {
        get => (double)GetValue(DiscountValueProperty);
        set => SetValue(DiscountValueProperty, value);
    }

    public int ProductId
    {
        get => (int)GetValue(ProductIdProperty);
        set => SetValue(ProductIdProperty, value);
    }

    public ProductVerticalVariantComponent()
	{
		InitializeComponent();
	}

    private async void Product_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            var product = await _productService.Get(ProductId);
            if (product != null)
            {
                await LastCheckedProductsCache.SaveLastCheckedProducts(product);
            }

            await Navigation.PushAsync(new ProductPage(ProductId));
        }
        catch (Exception)
        {

            await Navigation.PushAsync(new ProductPage(ProductId));
        }
    }
}