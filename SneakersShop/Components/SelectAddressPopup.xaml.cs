using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.Views;
using System.Collections.ObjectModel;

namespace SneakersShop.Components;

public partial class SelectAddressPopup
{
    public ObservableCollection<AddressModel> Addresses { get; }

    private readonly AddressModel SelectedAddress;
    private AddressModel _selectedAddress;

    public SelectAddressPopup(ObservableCollection<AddressModel> addresses, AddressModel selectedAddress)
    {
        InitializeComponent();
        Addresses = addresses;
        BindingContext = this;
        SelectedAddress = selectedAddress;
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is AddressModel selected)
        {
            SelectedAddress.IsSelected = false;
            _selectedAddress = selected;
        }
    }

    private async void Add_New_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Close();
        await Shell.Current.GoToAsync(nameof(CreateAddressPage));
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

    
}
