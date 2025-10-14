using SneakersShop.Models;
using SneakersShop.Views;
using System.Collections.ObjectModel;

namespace SneakersShop.Components.Popups;

public partial class AddressPopup
{
    public ObservableCollection<AddressesModel> Addresses { get; }
    private readonly AddressesModel SelectedAddress = new();
    private AddressesModel _selectedAddress = new();

    public AddressPopup(ObservableCollection<AddressesModel> addresses, AddressesModel selectedAddress)
	{
		InitializeComponent();
        Addresses = addresses;
        BindingContext = this;
        SelectedAddress = selectedAddress;
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is AddressesModel selected)
        {
            SelectedAddress.IsSelected = false;
            _selectedAddress = selected;
        }
    }

    private async void Add_New_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Close();
        await Shell.Current.GoToAsync(nameof(AddressCreatePage));
    }

    private void Cancel_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Close();
    }

    private void Accept_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (_selectedAddress != null)
        {
            Close(_selectedAddress);
        }
        else
        {
            Close();
        }
    }

    private async void Update_SwipeItem_Clicked(object sender, EventArgs e)
    {
        if (sender is SwipeItem swipeItem && swipeItem.BindingContext is AddressesModel address)
        {
            await Shell.Current.GoToAsync($"{nameof(AddressUpdatePage)}?Id={address.Id}");
        }
    }
}