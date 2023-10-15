using System.Text.Json;
using MovieStoreApi.Common.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CustomException ex)
        {
            await HandleException(context, ex);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        string error = "";

        // If the error sent is CustomException, take action accordingly
        if (ex is CustomException customException)
        {
            context.Response.StatusCode = customException.ErrorCode; // ErrorCode
            error = customException.Error; //Error Message

            _logger.LogError("Status : {0}" +
           "\n      Message: {1}" +
           "\n      Error: {2}" +
           "\n      Method: {3}" +
           "\n      Path: {4}", customException.ErrorCode, customException.Message, customException.Error, context.Request.Method, context.Request.Path);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            error = "Internal Server Error";
            _logger.LogError("Error : {0}" +
           "\n      Method: {1}" +
           "\n      Path: {2}", ex.Message, context.Request.Method, context.Request.Path);
        }

        var response = new { status = context.Response.StatusCode, error, message = ex.Message };
        var jsonResponse = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(jsonResponse);

    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}
