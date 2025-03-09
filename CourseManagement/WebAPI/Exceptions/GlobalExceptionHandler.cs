using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;


namespace WebAPI.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
        {
            var response = exception switch
            {
                NotFoundException => new { message = exception.Message, statusCode = 404 },
                ValidationException => new { message = exception.Message, statusCode = 400 },
                _ => new { message = "An error occurred", statusCode = 500 }
            };
            context.Response.StatusCode = response.statusCode;
            await context.Response.WriteAsJsonAsync(response);
            return true;
        }
    }

}
