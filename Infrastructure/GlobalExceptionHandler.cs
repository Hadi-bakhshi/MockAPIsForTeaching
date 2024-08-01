using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var exceptionDetails = GetExceptionDetails(exception);
        var problemDetails = new ProblemDetails
        {
            Status = exceptionDetails.Status,
            Type = exceptionDetails.Type,
            Title = exceptionDetails.Title,
            Detail = exceptionDetails.Details
        };
        if (exceptionDetails.Errors is not null)
        {
            problemDetails.Extensions["errors"] = exceptionDetails.Errors;
        }
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionDetails(
                StatusCodes.Status400BadRequest,
                "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                "Bad Request",
                "One or more validation errors has occurred",
               null
                ),
            _ => new ExceptionDetails(
                 StatusCodes.Status500InternalServerError,
                "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                "Confronted an unexpected server error",
                "An unexpected error has occurred",
                null
                ),
        };
    }
}

internal record ExceptionDetails(
    int Status,
    string Type,
    string Title,
    string Details,
    IEnumerable<object>? Errors
    );