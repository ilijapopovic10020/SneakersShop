using SneakersShop.ViewModels;

namespace SneakersShop.Views;

public partial class CheckoutPage : ContentPage
{
	private readonly CheckoutViewModel _vm;
	public CheckoutPage(CheckoutViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
        BindingContext = _vm;
    }

    private async void AddAddress_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddressCreatePage));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadAddressesCommand.ExecuteAsync(null);
    }

    private void CardNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;

        var cursorPos = entry.CursorPosition;

        var digits = new string(entry.Text.Where(char.IsDigit).ToArray());

        if (digits.Length > 16)
            digits = digits[..16];

        var formatted = string.Join(" ", Enumerable.Range(0, digits.Length / 4 + (digits.Length % 4 == 0 ? 0 : 1))
            .Select(i => digits.Skip(i * 4).Take(4))
            .Where(g => g.Any())
            .Select(g => string.Concat(g)));

        if (entry.Text == formatted)
            return;

        entry.Text = formatted;

        var diff = formatted.Length - digits.Length;
        var newPos = Math.Min(cursorPos + (formatted.Length > e.NewTextValue.Length ? 0 : 1), formatted.Length);
        entry.CursorPosition = newPos;
    }
}