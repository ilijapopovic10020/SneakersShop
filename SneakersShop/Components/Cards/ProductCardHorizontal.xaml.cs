using SneakersShop.Views;

namespace SneakersShop.Components.Cards;

public partial class ProductCardHorizontal : ContentView
{
    public static readonly BindableProperty ProductColorIdProperty =
        BindableProperty.Create(nameof(ProductColorId), typeof(int), typeof(ProductCardHorizontal), 0);

    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(ProductCardHorizontal), "");

    public static readonly BindableProperty BrandProperty =
        BindableProperty.Create(nameof(Brand), typeof(string), typeof(ProductCardHorizontal), "");

    public static readonly BindableProperty ImageProperty =
        BindableProperty.Create(nameof(Image), typeof(string), typeof(ProductCardHorizontal), "");

    public static readonly BindableProperty OldPriceProperty =
        BindableProperty.Create(nameof(OldPrice), typeof(decimal), typeof(ProductCardHorizontal), 0m);

    public static readonly BindableProperty NewPriceProperty =
        BindableProperty.Create(nameof(NewPrice), typeof(decimal), typeof(ProductCardHorizontal), 0m);

    public static readonly BindableProperty CountReviewProperty =
        BindableProperty.Create(nameof(CountReview), typeof(int), typeof(ProductCardHorizontal), 0);

    public static readonly BindableProperty AvgRatingProperty =
        BindableProperty.Create(nameof(AvgRating), typeof(decimal), typeof(ProductCardHorizontal), 0m);

    public static readonly BindableProperty HasDiscountProperty =
        BindableProperty.Create(nameof(HasDiscount), typeof(bool), typeof(ProductCardHorizontal), false);

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(string), typeof(ProductCardHorizontal), "");

    public int ProductColorId
    {
        get => (int)GetValue(ProductColorIdProperty);
        set => SetValue(ProductColorIdProperty, value);
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

    public string Image
    {
        get => (string)GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public decimal OldPrice
    {
        get => (decimal)GetValue(OldPriceProperty);
        set => SetValue(OldPriceProperty, value);
    }

    public decimal NewPrice
    {
        get => (decimal)GetValue(NewPriceProperty);
        set => SetValue(NewPriceProperty, value);
    }

    public int CountReview
    {
        get => (int)GetValue(CountReviewProperty);
        set => SetValue(CountReviewProperty, value);
    }

    public decimal AvgRating
    {
        get => (decimal)GetValue(AvgRatingProperty);
        set => SetValue(AvgRatingProperty, value);
    }

    public bool HasDiscount
    {
        get => (bool)GetValue(HasDiscountProperty);
        set => SetValue(HasDiscountProperty, value);
    }

    public string TextColor
    {
        get => (string)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public ProductCardHorizontal()
	{
		InitializeComponent();
	}

    private async void ProductDetail_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(ProductPage)}?ProductId={ProductColorId}");
    }
}