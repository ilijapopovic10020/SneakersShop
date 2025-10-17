using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class PasswordUpdatePage : ContentPage
{
	private readonly PasswordUpdateViewModel _vm;
    public PasswordUpdatePage(PasswordUpdateViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
        BindingContext = _vm;
    }
}