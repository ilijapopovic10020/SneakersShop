using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class ProfileUpdatePage : ContentPage
{
    private readonly ProfileUpdateViewModel _vm;
    public ProfileUpdatePage(ProfileUpdateViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadUserCommand.ExecuteAsync(null);
    }
}