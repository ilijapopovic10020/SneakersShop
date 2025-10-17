using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Components.Popups;
using SneakersShop.Helpers;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using SneakersShop.Views;

namespace SneakersShop.ViewModels
{
    public partial class OrdersViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;

        public OrdersViewModel(IOrderService orderService)
        {
            _orderService = orderService;

            orders = [];

            OrderOptions = [.. Enum.GetValues(typeof(OrderStatus))
                              .Cast<OrderStatus>()
                              .Select(os => new OrderStatusOption{Status = os})];

            SelectedOrderOption = OrderOptions.FirstOrDefault(x => x.Status == OrderStatus.Pending);
            SelectedOrderOption.IsSelected = true;
        }

        [ObservableProperty]
        private ObservableCollection<OrdersModel> orders;

        [ObservableProperty]
        private int totalCount;

        [ObservableProperty]
        private int currentPage = 1;

        [ObservableProperty]
        private int itemsPerPage = 10;

        [ObservableProperty]
        private int pagesCount;

        [ObservableProperty]
        private bool isPaginationAvailable = true;

        [ObservableProperty]
        private bool isNextPageAvailable = true;

        [ObservableProperty]
        private bool isPreviousPageAvailable = true;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private bool hasOrders = false;

        [ObservableProperty]
        private ObservableCollection<OrderStatusOption> orderOptions;

        [ObservableProperty]
        private OrderStatusOption selectedOrderOption;

        [RelayCommand]
        private async Task LoadOrders()
        {
            try
            {
                IsLoading = true;
                var orders = await _orderService.GetOrdersAsync(CurrentPage, ItemsPerPage);

                if (orders.Data != null)
                {
                    if(SelectedOrderOption.Status == OrderStatus.All)
                    {
                        Orders = [.. orders.Data];
                    }
                    else
                    {

                        Orders = [.. orders.Data.Where(x => x.OrderStatus == SelectedOrderOption.Status)];
                    }

                    TotalCount = orders.TotalCount;
                    PagesCount = orders.PagesCount;

                    IsPaginationAvailable = TotalCount > 0 && PagesCount > 1;

                    IsPreviousPageAvailable = CurrentPage > 1;

                    IsNextPageAvailable = CurrentPage != PagesCount;

                    HasOrders = TotalCount > 0;
                }
            }
            catch (TaskCanceledException)
            {
                var popup = new MessagePopup("Greška", "Veza sa serverom je prekinuta. Proverite internet konekciju i pokušajte ponovo.");
                await Shell.Current.ShowPopupAsync(popup);
            }
            catch (HttpRequestException)
            {
                var popup = new MessagePopup("Greška", "Veza sa serverom je prekinuta. Proverite internet konekciju i pokušajte ponovo.");
                await Shell.Current.ShowPopupAsync(popup);
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
        private async Task FilterOrders(OrderStatusOption status)
        {
            // označi novi kao selected
            foreach (var option in OrderOptions)
                option.IsSelected = false;

            status.IsSelected = true;
            SelectedOrderOption = status;

            CurrentPage = 1;
            await LoadOrders();
        }

        [RelayCommand]
        private async Task OrderDetail(int id)
        {
            await Shell.Current.GoToAsync($"{nameof(OrderPage)}?OrderId={id}");
        }

        [RelayCommand]
        private async Task NextPage()
        {
            if (CurrentPage < PagesCount)
            {
                CurrentPage++;
                await LoadOrders();
            }
        }

        [RelayCommand]
        private async Task PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadOrders();
            }
        }

        [RelayCommand]
        private async Task CancelOrder(int id)
        {
            try
            {
                var result = await _orderService.CancelOrderAsync(id);

                if (result)
                {
                    var popup = new MessagePopup("Uspeh", "Uspešno otkazivanje porudžbine.");
                    await Shell.Current.ShowPopupAsync(popup);
                    await LoadOrders();
                }
                else
                {
                    var popup = new MessagePopup("Greška", "Nije moguće otkazati porudžbinu. Molimo pokušajte kasnije.");
                    await Shell.Current.ShowPopupAsync(popup);
                }
            }
            catch (TaskCanceledException)
            {
                var popup = new MessagePopup("Greška", "Veza sa serverom je prekinuta. Proverite internet konekciju i pokušajte ponovo.");
                await Shell.Current.ShowPopupAsync(popup);
            }
            catch (HttpRequestException)
            {
                var popup = new MessagePopup("Greška", "Veza sa serverom je prekinuta. Proverite internet konekciju i pokušajte ponovo.");
                await Shell.Current.ShowPopupAsync(popup);
            }
            catch (Exception ex)
            {
                var popup = new MessagePopup("Greška", ex.Message);
                await Shell.Current.ShowPopupAsync(popup);
            }
        }
    }
}
