using Microsoft.AspNetCore.Http;

namespace WebApi.Infrastructure.Middleware
{
    public abstract class BaseMiddleware
    {
        protected RequestDelegate Next { get; private init; }

        public BaseMiddleware(RequestDelegate next) 
        {
            this.Next = next;
        }
    }
}