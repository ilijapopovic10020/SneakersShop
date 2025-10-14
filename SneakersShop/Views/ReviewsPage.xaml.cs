using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class ReviewsPage : ContentPage
{
	private readonly ReviewsViewModel _vm;
    public ReviewsPage(ReviewsViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadReviewsCommand.ExecuteAsync(null);
    }
}