using System.Net;

namespace SneakersShop.Exceptions
{
    public class UserFriendlyException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public UserFriendlyException(string message, HttpStatusCode statusCode) : base(message) { StatusCode = statusCode; }
    }
}
