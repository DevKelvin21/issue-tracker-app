using IssueTracker.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.API.Middleware;

/// <summary>
/// Global exception handler implementing RFC 9457 Problem Details for HTTP APIs
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Exception occurred: {Message}",
            exception.Message);

        var (statusCode, type, title, detail) = exception switch
        {
            NotFoundException notFound => (
                StatusCodes.Status404NotFound,
                "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
                "Resource Not Found",
                notFound.Message
            ),
            BadRequestException badRequest => (
                StatusCodes.Status400BadRequest,
                "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
                "Bad Request",
                badRequest.Message
            ),
            FluentValidation.ValidationException validation => (
                StatusCodes.Status400BadRequest,
                "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
                "Validation Error",
                string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
                "Internal Server Error",
                "An unexpected error occurred. Please try again later."
            )
        };

        var problemDetails = new ProblemDetails
        {
            Type = type,
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        if (System.Diagnostics.Activity.Current?.Id != null)
        {
            problemDetails.Extensions["requestId"] = System.Diagnostics.Activity.Current.Id;
        }

        problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
        problemDetails.Extensions["method"] = httpContext.Request.Method;

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
