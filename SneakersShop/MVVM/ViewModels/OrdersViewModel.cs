using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using SneakersShop.MVVM.Models.Search;
using SneakersShop.Services;
using System.Collections.ObjectModel;

namespace SneakersShop.MVVM.ViewModels
{
    public partial class OrdersViewModel : ObservableObject
    {
        private readonly OrderService _orderService;

        [ObservableProperty]
        private ObservableCollection<OrdersModel> orders;

        [ObservableProperty]
        private bool isloading = false;

        [ObservableProperty]
        private OrderModel order;

        [ObservableProperty]
        private bool showAllOrders;

        [ObservableProperty]
        private ObservableCollection<OrdersModel> filteredOrders;

        [ObservableProperty]
        private bool isReceivedDateVisible = true;

        public OrdersViewModel()
        {
            _orderService = new();
            Orders = [];
            FilteredOrders = [];
        }

        [RelayCommand]
        private async Task LoadOrders()
        {
            try
            {
                Isloading = true;
                var user = await SecureStorage.Default.GetUser();
                var search = new PagedSearchId
                {
                    Id = user.Id,
                    Page = 1,
                    PerPage = 15
                };
                var orders = await _orderService.Get(search);
                Orders = [.. orders];
            }
            catch (Exception ex)
            {
                await SnackbarHelper.ShowMessage(ex.Message);
            }
            finally
            {
                Isloading = false;
            }
        }

        partial void OnOrdersChanged(ObservableCollection<OrdersModel> value)
        {
            FilterOrders();
        }
        
        public string ToggleButtonText => ShowAllOrders ? "Prikaži samo na čekanju" : "Prikaži sve porudžbine";

        partial void OnShowAllOrdersChanged(bool value)
        {
            FilterOrders();
            OnPropertyChanged(nameof(ToggleButtonText));
        }

        private void FilterOrders()
        {
            if (Orders == null) return;
            if(ShowAllOrders)
            {
                FilteredOrders = [.. Orders];
            }
            else
            {
                FilteredOrders = [.. Orders.Where(x => x.OrderStatus == OrderStatus.Pending || x.OrderStatus == OrderStatus.Shipped)];
            }
        }

        [RelayCommand]
        private void ToggleShowAllOrders()
        {
            ShowAllOrders = !ShowAllOrders;
        }

        [RelayCommand]
        private async Task LoadOrder(int id)
        {
            try
            {
                Order = await _orderService.Get(id);
                if (Order.ReceivedDate != null)
                {
                    IsReceivedDateVisible = true;
                }
                else
                {
                    IsReceivedDateVisible = false;
                }
            }
            catch (Exception ex)
            {

                await SnackbarHelper.ShowMessage(ex.Message);
            }
        }
    }
}
