using SneakersShop.MVVM.ViewModels;
using System.Threading.Tasks;
using UraniumUI.Pages;

namespace SneakersShop.MVVM.Views;

public partial class ReviewsPage : UraniumContentPage
{
    public ReviewsViewModel ViewModel { get; }
    public int Id { get; }
    public ReviewsPage(int id)
    {
        InitializeComponent();
        ViewModel = new ReviewsViewModel();
        BindingContext = ViewModel;
        Id = id;



    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        await ViewModel.LoadReviewsCommand.ExecuteAsync(Id);
        ViewModel.LoadStarsCommand.Execute(null);
    }

    private async void Create_Review_Page_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        CreateReviewBottomSheet.IsPresented = true;
        CreateReviewBottomSheet.IsVisible = true;
    }

    private async void Back_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}