using SneakersShop.Exceptions;
using SneakersShop.Extensions;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace SneakersShop.Services.Implementations
{
    public class CartService(HttpClient httpClient) : ICartService
    {

        private readonly HttpClient _httpClient = httpClient;
        private readonly ObservableCollection<CartModel> cartItems = [];

        public ObservableCollection<CartModel> CartItems => cartItems;

        public async Task<List<CartModel>> GetCartItemsAsync()
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Get, "api/cart");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var cart = await response.Content.ReadFromJsonAsync<List<CartModel>>();

            return cart ?? throw new UserFriendlyException("Nije moguće učitati proizvode iz korpe. Molimo pokušajte ponovo.", System.Net.HttpStatusCode.BadRequest);
        }

        public async Task<bool> UpsertAsync(List<CartModel> cart)
        {
            var user = await SecureStorage.Default.GetUser();

            var json = JsonSerializer.Serialize(cart);

            var request = new HttpRequestMessage(HttpMethod.Post, "api/cart")
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.AccessToken);

            var response = await _httpClient.SendAsync(request);

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
            if(cartItems.Count == 0)
            {
                var serverItems = await GetCartItemsAsync();
                LoadFromServer(serverItems);
            }

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

            await UpsertAsync([.. cartItems]);
        }

        public async Task RemoveItem(CartModel cartItem)
        {
            cartItems.Remove(cartItem);
            await UpsertAsync(cartItems.ToList());
        }

        public async Task RemoveItems(IEnumerable<CartModel> cartItemsToRemove)
        {
            foreach (var item in cartItemsToRemove)
            {
                cartItems.Remove(item);
            }

            await UpsertAsync(cartItems.ToList());
        }

        public async Task ClearCart()
        {
            cartItems.Clear();
            await UpsertAsync(cartItems.ToList());
        }
    }
}
