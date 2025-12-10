using System.Net;
using System.Text.Json;

namespace BugManagement.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;


        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //var result = JsonSerializer.Serialize(new { message = "An unexpected error occurred." });
                var result = JsonSerializer.Serialize(new { message = ex.Message });
                await context.Response.WriteAsync(result);
            }
        }
    }
}
