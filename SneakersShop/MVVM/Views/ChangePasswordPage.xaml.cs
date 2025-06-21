namespace SneakersShop.MVVM.Views;

public partial class ChangePasswordPage : ContentPage
{
	public ChangePasswordPage()
	{
		InitializeComponent();
	}

    private async void Back_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		await Navigation.PopAsync();
    }
}