using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.ViewModels;

namespace SneakersShop.MVVM.Views;

public partial class UpdateProfilePage : ContentPage
{
    private readonly UpdateUserViewModel _viewModel;
    public UserModel _user { get; }
	public UpdateProfilePage (UserModel user)
	{
		InitializeComponent();
        _viewModel = new UpdateUserViewModel();
        BindingContext = _viewModel;
        _user = user;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.User = _user;
        _viewModel.FirstName = _user.FirstName;
        _viewModel.LastName = _user.LastName;
        _viewModel.Phone = _user.Phone;
        _viewModel.Email = _user.Email;
        _viewModel.DisplayedImageSource = ImageSource.FromUri(new Uri(_user.FullImageUrl));

    }

    private async void Back_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		await Navigation.PopAsync();
    }
}