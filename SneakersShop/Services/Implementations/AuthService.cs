using SneakersShop.Exceptions;
using SneakersShop.Extensions;
using SneakersShop.Models;
using SneakersShop.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;

namespace SneakersShop.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthInfoModel> LoginAsync(AuthModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth?revokeOld=false", model);

            if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
                throw new UserFriendlyException("Neispravni podaci za prijavu. Proverite korisničko ime i lozinku.", HttpStatusCode.UnprocessableEntity);

            ExceptionHelper.ThrowIfUnsuccessful(response);

            var tokenData = await response.Content.ReadFromJsonAsync<TokenResponseModel>();
            if (tokenData?.AccessToken == null)
                throw new UserFriendlyException("Došlo je do greške pri prijavi. Pokušajte ponovo.", HttpStatusCode.InternalServerError);

            var claims = new JwtSecurityTokenHandler().ReadJwtToken(tokenData.AccessToken);
            var userIdClaim = claims.Claims.FirstOrDefault(c => c.Type == "Id");
            var expClaim = claims.Claims.FirstOrDefault(c => c.Type == "exp");

            if (userIdClaim == null || expClaim == null)
                throw new UserFriendlyException("Došlo je do greške pri obradi tokena. Pokušajte ponovo.", HttpStatusCode.InternalServerError);

            return new AuthInfoModel
            {
                Id = int.Parse(userIdClaim.Value),
                AccessToken = tokenData.AccessToken,
                RefreshToken = tokenData.RefreshToken,
                LoginExparation = double.Parse(expClaim.Value).ToDateTime()
            };
        }
    }
}
