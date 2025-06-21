using SneakersShop.MVVM.Models;
using SneakersShop.Helpers;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SneakersShop.Services
{
    public class SizeService : BaseService
    {
        public async Task<IEnumerable<SizeModel>> Get()
        {
            var response = await _httpClient.GetAsync("api/sizes");

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var sizes = await response.Content.ReadFromJsonAsync<IEnumerable<SizeModel>>();

            if (sizes == null)
            {
                throw new UserFriendlyException("Nije moguće učitati veličine.", System.Net.HttpStatusCode.BadRequest);
            }

            return sizes;
        }
    }
}
