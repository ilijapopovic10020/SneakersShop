using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class ReviewCreatePage : ContentPage
{
	public ReviewCreatePage(ReviewCreateViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

		vm.LoadStarsCommand.Execute(null);
    }
}