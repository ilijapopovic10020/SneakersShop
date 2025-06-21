using SneakersShop.Helpers;
using SneakersShop.MVVM.Models;
using System.Net.Http.Json;

namespace SneakersShop.Services
{
    public class ColorService : BaseService
    {
        public async Task<IEnumerable<ColorModel>> Get()
        {
            var response = await _httpClient.GetAsync("api/colors");

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var colors = await response.Content.ReadFromJsonAsync<IEnumerable<ColorModel>>();

            if (colors == null)
                throw new UserFriendlyException("Nije moguće učitati boje.", System.Net.HttpStatusCode.BadRequest);

            return colors;
        }
    }
}
