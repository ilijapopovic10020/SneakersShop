using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Helpers;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using SneakersShop.Validators.Interfaces;
using SneakersShop.Views;
using System.Collections.ObjectModel;

namespace SneakersShop.ViewModels
{
    public partial class CheckoutViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IOrderService _orderService;
        private readonly IAddressService _addressService;
        private readonly ICheckoutViewModelValidator _validator;

        private readonly CartViewModel _cartViewModel;

        public CheckoutViewModel(IOrderService orderService, IAddressService addressService, ICheckoutViewModelValidator validator, CartViewModel cartViewModel)
        {
            _orderService = orderService;
            _addressService = addressService;
            _validator = validator;

            _cartViewModel = cartViewModel;

            CardHolder.Value = string.Empty;
            CardNumber.Value = string.Empty;
            Cvv.Value = string.Empty;
            Expiration.Value = string.Empty;
            Notes.Value = string.Empty;

            CardHolder.ValueChanged += (_) => Validator();
            CardNumber.ValueChanged += (_) => Validator();
            Cvv.ValueChanged += (_) => Validator();
            Expiration.ValueChanged += (_) => Validator();
            Notes.ValueChanged += (_) => Validator();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("OrderItems", out var items))
                OrderItems = items as List<CreateOrderItem> ?? [];

            if (query.TryGetValue("TotalPrice", out var price))
                TotalPrice = (double)(price ?? 0);

            if (query.TryGetValue("CartQuantity", out var quantity))
                CartQuantity = (int)(quantity ?? 0);
        }

        [ObservableProperty]
        private List<CreateOrderItem> orderItems = [];

        [ObservableProperty]
        private ObservableCollection<AddressesModel> addresses = [];

        [ObservableProperty]
        private AddressesModel selectedAddress = new();

        [ObservableProperty]
        private bool hasAnyAddresses = true;

        [ObservableProperty]
        private double totalPrice;

        [ObservableProperty]
        private int cartQuantity;

        [ObservableProperty]
        private bool isLoading = false;

        public List<PaymentTypeOption> PaymentOptions { get; } = [.. Enum.GetValues<PaymentType>().Select(pt => new PaymentTypeOption { PaymentType = pt })];

        [ObservableProperty]
        private PaymentType selectedPaymentType = PaymentType.Card;

        [ObservableProperty]
        private bool isCardPaymentSelected = true;

        public BindableField<string?> Notes { get; set; } = new();
        public BindableField<string?> CardHolder { get; set; } = new();
        public BindableField<string?> Cvv { get; set; } = new();
        public BindableField<string?> CardNumber { get; set; } = new();
        public BindableField<string?> Expiration { get; set; } = new();

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
                IsLoading = true;
                var addresses = await _addressService.GetAddressesAsync();

                if (addresses != null)
                {
                    Addresses = [.. addresses];

                    HasAnyAddresses = Addresses.Count > 0;

                    SelectedAddress = Addresses.FirstOrDefault(a => a.IsDefault == true);

                    if (SelectedAddress != null)
                        SelectedAddress.IsSelected = true;
                }
                else
                {
                    var popup = new MessagePopup("Greška", "Došlo je do greške prilikom učitavanja adresa. Molimo pokušajte ponovo.");
                    await Shell.Current.ShowPopupAsync(popup);
                }

            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task ShowAddressSelection()
        {
            var popup = new AddressPopup(Addresses, SelectedAddress);
            var result = await Shell.Current.ShowPopupAsync(popup);

            if (result is AddressesModel selected)
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

                var result = await _orderService.CreateOrderAsync(order);

                if (result)
                {
                    await _cartViewModel.RemovePurchasedItemsAsync();

                    await Shell.Current.GoToAsync(nameof(OrderSuccessPage));
                }
                else
                {
                    var popup = new MessagePopup("Greška", "Došlo je do greške prilikom kreiranja porudžbine. Molimo pokušajte ponovo.");
                    await Shell.Current.ShowPopupAsync(popup);
                }
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }
    }
}
