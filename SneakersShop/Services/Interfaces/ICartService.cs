using SneakersShop.Models;
using System.Collections.ObjectModel;

namespace SneakersShop.Services.Interfaces
{
    public interface ICartService
    {
        ObservableCollection<CartModel> CartItems { get; }

        Task<List<CartModel>> GetCartItemsAsync();
        Task<bool> UpsertAsync(List<CartModel> cartModels);

        ObservableCollection<CartModel> GetItems();
        void LoadFromServer(List<CartModel> cartModels);

        Task AddItem(CartModel cartItem);
        Task RemoveItem(CartModel cartItem);
        Task RemoveItems(IEnumerable<CartModel> cartItemsToRemove);
        Task ClearCart();
    }
}
