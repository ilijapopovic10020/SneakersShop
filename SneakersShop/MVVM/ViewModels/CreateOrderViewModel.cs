using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components;
using SneakersShop.Helpers;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.Views;
using SneakersShop.Services;
using SneakersShop.Validators;
using System.Collections.ObjectModel;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class CreateOrderViewModel : ObservableObject
    {
        private readonly AddressService _addressService;
        private readonly OrderService _orderService;
        private readonly CreateOrderViewModelValidator _validator;

        public CartViewModel CartViewModel;

        [ObservableProperty]
        private List<CreateOrderItem> orderItems = [];

        [ObservableProperty]
        private string totalPrice;

        [ObservableProperty]
        private string cartQuantity;

        public List<PaymentTypeOption> PaymentOptions { get; } = [.. Enum.GetValues<PaymentType>().Select(pt => new PaymentTypeOption { PaymentType = pt })];

        [ObservableProperty]
        private PaymentType selectedPaymentType = PaymentType.Card;

        [ObservableProperty]
        private bool isCardPaymentSelected = true;

        public BindableField<string> Notes { get; set; } = new();
        public BindableField<string> CardHolder { get; set; } = new();
        public BindableField<string> Cvv { get; set; } = new();
        public BindableField<string> CardNumber { get; set; } = new();
        public BindableField<string> Expiration { get; set; } = new();

        [ObservableProperty]
        private ObservableCollection<AddressModel> addresses;

        [ObservableProperty]
        private AddressModel selectedAddress;

        [ObservableProperty]
        private bool hasAnyAddresses = true;

        

        public CreateOrderViewModel()
        {
            _addressService = new();
            _orderService = new();
            _validator = new();

            Notes.Value = string.Empty;
            CardHolder.Value = string.Empty;
            CardNumber.Value = string.Empty;
            Cvv.Value = string.Empty;
            Expiration.Value = string.Empty;

            CardHolder.ValueChanged += (_) => Validator();
            CardNumber.ValueChanged += (_) => Validator();
            Cvv.ValueChanged += (_) => Validator();
            Expiration.ValueChanged += (_) => Validator();
            Notes.ValueChanged += (_) => Validator();
        }

        private void Validator()
        {
            var result = _validator.Validate(this);

            CardHolder.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("CardHolder"))?.ErrorMessage ?? string.Empty;

            CardNumber.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("CardNumber"))?.ErrorMessage ?? string.Empty;

            Cvv.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Cvv"))?.ErrorMessage ?? string.Empty;

            Expiration.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Expiration"))?.ErrorMessage ?? string.Empty;

            Notes.Error = result.Errors.FirstOrDefault(x => x.PropertyName.Contains("Notes"))?.ErrorMessage ?? string.Empty;
        }

        [RelayCommand]
        private void SelectPayment(PaymentType type)
        {
            SelectedPaymentType = type;

            IsCardPaymentSelected = SelectedPaymentType == PaymentType.Card;
        }

        [RelayCommand]
        private async Task LoadAddresses()
        {
            try
            {

                Addresses = new ObservableCollection<AddressModel>(await _addressService.Get());

                HasAnyAddresses = Addresses.Count > 0;

                SelectedAddress = Addresses.FirstOrDefault(a => a.IsDefault == true) ?? Addresses.FirstOrDefault();

                if (SelectedAddress is not null)
                    SelectedAddress.IsSelected = true;


            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                var result = await App.Current.MainPage.ShowPopupAsync(popup);
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

        [RelayCommand]
        private async Task CreateOrder()
        {
            //Todo: ne radi porudžbina izbacuje null reference exception - šala?
            //nije šala nešto je do servera nije dobar regex vrv jer sam stavio 4 i kaže da su samo visa i master card podržane ništa mi nije jasno 
            try
            {
                var order = new CreateOrderModel
                {
                    PaymentType = (int)SelectedPaymentType,
                    Notes = Notes.Value,
                    CardHolder = CardHolder.Value,
                    CardNumber = CardNumber.Value,
                    Cvv = Cvv.Value,
                    Expiration = Expiration.Value,
                    AddressId = SelectedAddress.Id,
                    Items = OrderItems.Select(oi => new CreateOrderItem
                    {
                        ProductColorId = oi.ProductColorId,
                        SizeId = oi.SizeId,
                        Quantity = oi.Quantity
                    })
                };

                var result = await _orderService.Post(order);

                if (result)
                {
                    await CartViewModel.RemovePurchasedItems();

                    await Application.Current.MainPage.Navigation.PushAsync(new OrderSuccessPage());
                }
                else
                {
                    await SnackbarHelper.ShowMessage("Greška prilikom poručivanja.");
                }
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                var result = await App.Current.MainPage.ShowPopupAsync(popup);
            }
        }
    }
}