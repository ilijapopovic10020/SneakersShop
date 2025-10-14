using SneakersShop.Exceptions;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SneakersShop.Services.Implementations
{
    public class ColorService : IColorService
    {
        private readonly HttpClient _httpClient;

        public ColorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<ColorsModel>> GetColorsAsync()
        {
            var response = await _httpClient.GetAsync("/api/colors");

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var colors = await response.Content.ReadFromJsonAsync<IEnumerable<ColorsModel>>() ??
                throw new UserFriendlyException("Nije moguće učitati brendove.", HttpStatusCode.BadRequest);

            return colors;
        }
    }
}
