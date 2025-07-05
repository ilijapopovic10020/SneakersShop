using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SneakersShop.Services
{
    public class CartService : BaseService
    {
        private readonly HttpService _httpService = new();
        private static CartService? instance;

        private readonly ObservableCollection<CartModel> cartItems;

        public CartService()
        {
            cartItems = [];
        }

        public static CartService Instance => instance ??= new CartService();

        public ObservableCollection<CartModel> CartItems => cartItems;

        public async Task<List<CartModel>> Get()
        {
            var user = await SecureStorage.Default.GetUser();
            var request = new HttpRequestMessage(HttpMethod.Get, "api/cart");

            var response = await _httpService.SendWithAutoRefresh(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CartModel>>(content) ?? [];
        }

        public async Task<bool> Upsert(List<CartModel> cart)
        {
            var user = await SecureStorage.Default.GetUser();
            var json = JsonSerializer.Serialize(cart);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "api/cart")
            {
                Content = content
            };

            var response = await _httpService.SendWithAutoRefresh(request);
            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public ObservableCollection<CartModel> GetItems()
        {
            return cartItems;
        }

        public void LoadFromServer(List<CartModel> itemsFromServer)
        {
            cartItems.Clear();
            foreach (var item in itemsFromServer)
            {
                cartItems.Add(item);
            }
        }

        public async Task AddItem(CartModel cartItem)
        {
            var existingItem = cartItems.FirstOrDefault(x =>
                x.Product.Id == cartItem.Product.Id &&
                x.Size.Id == cartItem.Size.Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
               cartItems.Add(cartItem);
            }

            await Upsert([.. cartItems]);
        }

        public async Task RemoveItem(CartModel cartItem)
        {
            cartItems.Remove(cartItem);
            await Upsert([.. cartItems]);
        }

        public async Task RemoveItems(IEnumerable<CartModel> cartItemsToRemove)
        {
            foreach(var item in cartItemsToRemove)
            {
                cartItems.Remove(item);
            }

            await Upsert([.. cartItems]);
        }

        public async Task ClearCart()
        {
            cartItems.Clear();
            await Upsert([.. cartItems]);
        }
    }
}
