using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace InventoryTracker.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails();
            switch (exception)
            {
                case ValidationException validationException:
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation failed",
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                        Extensions =
                        {
                            ["errors"] = validationException.Errors
                                        .GroupBy(e => e.PropertyName)
                                        .ToDictionary(
                                                    g => g.Key,
                                                    g => g.Select(e => e.ErrorMessage).ToArray())
                        }
                    };
                    break;
                case InvalidOperationException:
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Business rule violation",
                        Detail = exception.Message
                    };
                    break;
                default:
                    problemDetails = null;
                    break;
            }
            if (problemDetails is null)
            {
                // Unhandled exceptions — log and return generic 500
                _logger.LogError(exception, "Unhandled exception occurred");
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred"
                };
            }

            httpContext.Response.StatusCode = problemDetails.Status ?? 500;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
