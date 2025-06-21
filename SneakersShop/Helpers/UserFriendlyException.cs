using System.Net;

namespace SneakersShop.Helpers
{
    public class UserFriendlyException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public UserFriendlyException(string message, HttpStatusCode statusCode) : base(message) { StatusCode = statusCode; }
    }
}
