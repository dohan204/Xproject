using System.Net;
using System.Text.Json;
using TestX.api.CustomException;

namespace TestX.api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            if (ex is ApiException apiEx)
            {
                context.Response.StatusCode = (int)apiEx.StatusCode;

                var response = new
                {
                    error = apiEx.ErrorMessage,
                    message = apiEx.Message,
                    details = apiEx is ValidateException valEx ? valEx.Errors : null
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            // fallback cho lỗi hệ thống
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var defaultResponse = new
            {
                error = "INTERNAL_ERROR",
                message = "Có lỗi đã xảy ra, vui lòng thử lại sau."
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(defaultResponse));
        }
    }
}
