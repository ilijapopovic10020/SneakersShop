using SneakersShop.Models;
using SneakersShop.Models.Search;

namespace SneakersShop.Services.Interfaces
{
    public interface IOrderService
    {
        Task<PagedModel<OrdersModel>> GetOrdersAsync(int? page, int? perPage);
        Task<OrderModel> GetOrderByIdAsync(int id);

        Task<bool> CreateOrderAsync(CreateOrderModel orderModel);
        Task<bool> CancelOrderAsync(int id);
    }
}
