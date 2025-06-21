using SneakersShop.Helpers;
using SneakersShop.Helpers.Extensions;
using SneakersShop.MVVM.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;

namespace SneakersShop.Services
{
    internal class UserService : BaseService
    {
        private class TokenResponse
        {
            public string? Token { get; set; }
        }

        public async Task<LoginModel> Login(string username, string password)
        {
            var payload = new { username, password };
            var response = await _httpClient.PostAsJsonAsync("api/auth", payload);

            if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                throw new UserFriendlyException("Neispravni podaci za prijavu. Proverite korisničko ime i lozinku.", HttpStatusCode.UnprocessableEntity);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var tokenData = await response.Content.ReadFromJsonAsync<TokenResponse>();
            if (tokenData?.Token == null)
                throw new UserFriendlyException("Došlo je do greške pri prijavi. Pokušajte ponovo.", HttpStatusCode.InternalServerError);

            var claims = new JwtSecurityTokenHandler().ReadJwtToken(tokenData.Token);
            var userIdClaim = claims.Claims.FirstOrDefault(c => c.Type == "Id");
            var expClaim = claims.Claims.FirstOrDefault(c => c.Type == "exp");

            if (userIdClaim == null || expClaim == null)
                throw new UserFriendlyException("Došlo je do greške pri obradi tokena. Pokušajte ponovo.", HttpStatusCode.InternalServerError);

            return new LoginModel
            {
                Id = int.Parse(userIdClaim.Value),
                Token = tokenData.Token,
                LoginExparation = double.Parse(expClaim.Value).ToDateTime()
            };
        }

        public async Task<bool> Register(RegisterModel register)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", register);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<UserModel> Get(int id)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/users/{id}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Token);

            var response = await _httpClient.SendAsync(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var userData = await response.Content.ReadFromJsonAsync<UserModel>();

            if (userData == null)
                throw new UserFriendlyException("Korisnički podaci nisu pronađeni.", HttpStatusCode.NotFound);

            return userData;
        }

        public async Task<bool> Put(UserUpdateModel userUpdate)
        {
            var user = await SecureStorage.Default.GetUser();

            var request = new HttpRequestMessage(HttpMethod.Put, $"api/users/{user.Id}")
            {
                Content = JsonContent.Create(userUpdate)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Token);

            var response = await _httpClient.SendAsync(request);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            return response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.OK;
        }
    }
}
