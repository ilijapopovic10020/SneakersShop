using SneakersShop.MVVM.ViewModels;

namespace SneakersShop.MVVM.Views;

public partial class CreateAddressPage : ContentPage
{
	public CreateAddressPage()
	{
		InitializeComponent();
		((CreateAddressViewModel)BindingContext).LoadCitiesCommand.ExecuteAsync(null);
    }

    private async void Back_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		await Shell.Current.GoToAsync("..");
    }
}