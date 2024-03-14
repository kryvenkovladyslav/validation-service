using Microsoft.AspNetCore.Http;

namespace WebApi.Infrastructure.Middleware
{
    public abstract class BaseMiddleware
    {
        public RequestDelegate Next { get; set; }

        public BaseMiddleware(RequestDelegate next) 
        {
            this.Next = next;
        }
    }
}