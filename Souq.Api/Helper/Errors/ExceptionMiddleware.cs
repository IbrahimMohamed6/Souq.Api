using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace Souq.Api.Helper.Errors
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // تحديد النوع وحالة الـ StatusCode بناءً على نوع الاستثناء
            var (statusCode, message) = exception switch
            {
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access."),
                KeyNotFoundException => (HttpStatusCode.NotFound, "Resource not found."),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            context.Response.StatusCode = (int)statusCode;

            // تسجيل الخطأ
            _logger.LogError(exception, "An error occurred");

            // إنشاء رد يحتوي على الرسالة والتفاصيل
            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Details = _environment.IsDevelopment() ? exception.StackTrace : "An internal error occurred."
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            // إرسال الرد
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
