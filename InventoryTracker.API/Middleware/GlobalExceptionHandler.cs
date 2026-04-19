using FluentValidation;
using InventoryTracker.Application.Common.Exceptions;
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
            ProblemDetails problemDetails;
            switch (exception)
            {
                case ValidationException validationException:
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation failed",
                        Detail = "One or more validation errors occurred.",
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
                    };

                    problemDetails.Extensions["errors"] = validationException.Errors
                        .GroupBy(e => string.IsNullOrWhiteSpace(e.PropertyName) ? "General" : e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray());

                    break;
                case InvalidOperationException invalidOperationException:
                    _logger.LogError(invalidOperationException, "System configuration or invalid runtime state");
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Application error",
                        Detail = "An internal application error occurred.",
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1"
                    };
                    break;
                case RecordNotFoundException notFoundException:
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Resource not found",
                        Detail = notFoundException.Message,
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5"
                    };
                    break;
                case BusinessException businessException:
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Business rule violation",
                        Detail = businessException.Message,
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10"
                    };
                    break;
                case ConflictException conflictException:
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "Conflict occurred",
                        Detail = conflictException.Message,
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10"
                    };
                    break;
                default:
                    _logger.LogError(exception, "Unhandled exception occurred");
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "An unexpected error occurred",
                        Detail = "An unexpected server error occurred.",
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1"
                    };
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status ?? 500;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
