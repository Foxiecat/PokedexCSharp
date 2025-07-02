using Microsoft.AspNetCore.Diagnostics;

namespace src.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception,
            "Could not process a request on Machine {Machi8neName}. TraceID={TraceID}",
            Environment.MachineName, httpContext.TraceIdentifier);

        (int statusCode, string? title) = MapException(exception);

        await Results.Problem(
            title: title,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>()
            {
                { "traceID",  httpContext.TraceIdentifier }
            }).ExecuteAsync(httpContext);
        return true;
    }

    // TODO: Always add new found Exceptions to this function
    private static (int StatusCodes, string? title) MapException(Exception exception)
    {
        return exception switch
        {
            BadHttpRequestException => (StatusCodes.Status400BadRequest, $"Bad Request: {exception.Message}"),
            _ => (StatusCodes.Status500InternalServerError, $"Internal Server Error: {exception.Message}")
        };
    }
}