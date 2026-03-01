using Telemetry.Application.Exceptions;
using Telemetry.Domain.Exceptions;

namespace Telemetry.Api.Middlewares
{
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Log com níveis diferenciados conforme o tipo de exceção
                switch (ex)
                {
                    case BusinessException:
                        _logger.LogWarning(ex, "Exceção de negócio capturada.");
                        break;
                    case ApplicationTelemetryException:
                        _logger.LogInformation(ex, "Ocorreu um erro de validação.");
                        break;
                    default:
                        _logger.LogError(ex, "Ocorreu um erro não tratado.");
                        break;
                }

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode = exception switch
            {
                BusinessException => StatusCodes.Status400BadRequest,
                ApplicationTelemetryException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                status = statusCode,
                error = exception.Message
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
