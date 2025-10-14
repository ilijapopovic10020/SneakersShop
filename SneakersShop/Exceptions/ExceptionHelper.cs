using System.Net;

namespace SneakersShop.Exceptions
{
    public static class ExceptionHelper
    {
        public static void ThrowIfUnsuccessful(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var message = response.StatusCode switch
            {
                HttpStatusCode.BadRequest => "Neispravni podaci. Proverite unete informacije i pokušajte ponovo.",
                HttpStatusCode.Unauthorized => "Niste autorizovani za ovu akciju. Prijavite se ponovo.",
                HttpStatusCode.Forbidden => "Nemate prava pristupa ovoj funkcionalnosti.",
                HttpStatusCode.NotFound => "Traženi resurs nije pronađen.",
                HttpStatusCode.UnprocessableEntity => "Neispravni podaci za prijavu. Proverite korisničko ime i lozinku.",
                HttpStatusCode.InternalServerError => "Došlo je do greške na serveru. Pokušajte kasnije.",
                _ => "Došlo je do neočekivane greške. Molimo pokušajte ponovo."
            };

            throw new UserFriendlyException(message, response.StatusCode);
        }
    }
}
