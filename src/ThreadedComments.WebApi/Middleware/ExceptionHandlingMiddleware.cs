using System.Text.Json;
using ThreadedComments.Application.Common.Exceptions;
using ThreadedComments.WebApi.Contracts.Errors;

namespace ThreadedComments.WebApi.Middleware;


public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger
    )
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
        catch(Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception occurred");

            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, error) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
            ValidationException  => (StatusCodes.Status400BadRequest, "Bad Request"),
            ConflictException  => (StatusCodes.Status409Conflict, "Conflict"),
            ForbiddenException  => (StatusCodes.Status403Forbidden, "Forbidden"),
            ArgumentException   => (StatusCodes.Status400BadRequest, "Bad Request"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        var response = new ErrorResponse
        {
            Status = statusCode,
            Error = error,
            Message = exception.Message,
            TraceId = context.TraceIdentifier  
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}