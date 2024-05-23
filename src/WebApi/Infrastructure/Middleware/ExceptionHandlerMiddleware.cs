using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApi.Infrastructure.Middleware
{
    public sealed class ExceptionHandlerMiddleware : BaseMiddleware
    {
        private readonly string contentType;

        private readonly ILogger<ExceptionHandlerMiddleware> logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger) : base(next) 
        {
            this.logger = logger;
            this.contentType = "application/json";
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.Next(context);
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception.Message);

                var response = context.Response;
                response.ContentType = this.contentType;

                var problemDetail = new ProblemDetails
                {
                    Detail = exception.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error",
                };

                await response.WriteAsync(JsonSerializer.Serialize(problemDetail));
                return;
            }
        }
    }
}