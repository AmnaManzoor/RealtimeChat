using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace RealtimeChat.API.Middleware;

/// <summary>
/// Handles exceptions and returns Problem Details responses.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">Next request delegate.</param>
    /// <param name="logger">Logger instance.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, Serilog.ILogger logger)
    {
        _next = next;
        _logger = logger.ForContext<ExceptionHandlingMiddleware>();
    }

    /// <summary>
    /// Executes the middleware.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await WriteProblemDetailsAsync(context, ex);
        }
        catch (InvalidOperationException ex)
        {
            await WriteProblemDetailsAsync(context, ex, StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception");
            await WriteProblemDetailsAsync(context, ex, StatusCodes.Status500InternalServerError);
        }
    }

    private static Task WriteProblemDetailsAsync(HttpContext context, ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(e => e.PropertyName ?? string.Empty)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).Distinct().ToArray());

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Detail = "One or more validation errors occurred.",
            Type = "https://datatracker.ietf.org/doc/html/rfc7807"
        };

        problemDetails.Extensions["errors"] = errors;
        return WriteProblemDetailsAsync(context, problemDetails);
    }

    private static Task WriteProblemDetailsAsync(HttpContext context, Exception exception, int statusCode)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = statusCode == StatusCodes.Status500InternalServerError ? "Server error" : "Request error",
            Detail = exception.Message,
            Type = "https://datatracker.ietf.org/doc/html/rfc7807"
        };

        return WriteProblemDetailsAsync(context, problemDetails);
    }

    private static Task WriteProblemDetailsAsync(HttpContext context, ProblemDetails problemDetails)
    {
        context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/problem+json";
        return context.Response.WriteAsJsonAsync(problemDetails);
    }
}
