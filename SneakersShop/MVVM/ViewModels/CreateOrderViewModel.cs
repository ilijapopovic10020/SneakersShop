using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components;
using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using SneakersShop.Services;
using System.Collections.ObjectModel;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class CreateOrderViewModel : ObservableObject
    {
        private readonly UserService _userService = new();

        [ObservableProperty]
        private List<CreateOrderItem> orderItems = [];
        [ObservableProperty]
        private string totalPrice;
        [ObservableProperty]
        private string cartQuantity;

        [ObservableProperty]
        private PaymentType selectedPaymentType = PaymentType.Card;

        [ObservableProperty]
        private bool isCardPaymentSelected = true;

        [ObservableProperty]
        private UserModel user;

        [ObservableProperty]
        private string? notes;
        [ObservableProperty]
        private string? cardHolder;
        [ObservableProperty]
        private string? cardNumber;
        [ObservableProperty]
        private string? cvv;
        [ObservableProperty]
        private string? expiration;

        [ObservableProperty]
        private ObservableCollection<AddressModel> addresses;

        [ObservableProperty]
        private AddressModel selectedAddress;

        [ObservableProperty]
        private bool hasAnyAddresses = true;

        public List<PaymentTypeOption> PaymentOptions { get; } = [.. Enum.GetValues<PaymentType>().Select(pt => new PaymentTypeOption { PaymentType = pt })];

        [RelayCommand]
        private void SelectPayment(PaymentType type)
        {
            SelectedPaymentType = type;

            IsCardPaymentSelected = SelectedPaymentType == PaymentType.Card;
        }

        [RelayCommand]
        private async Task LoadUser()
        {
            try
            {
                var user = await SecureStorage.Default.GetUser();
                User = await _userService.Get(user.Id);
                Addresses = [.. User.Addresses];
                HasAnyAddresses = Addresses.Count > 0;
                SelectedAddress = Addresses.FirstOrDefault(a => a.IsDefault == true) ?? Addresses.FirstOrDefault();
                SelectedAddress.IsSelected = true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        [RelayCommand]
        private async Task ShowAddressSelection()
        {
            var popup = new SelectAddressPopup(Addresses, SelectedAddress);
            var result = await App.Current.MainPage.ShowPopupAsync(popup);

            if (result is AddressModel selected)
            {
                SelectedAddress = selected;
                SelectedAddress.IsSelected = true;
            }
        }
    }
}