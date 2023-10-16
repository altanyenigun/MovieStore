using FluentValidation;
using MovieStoreApi.Common.Exceptions;
using MovieStoreApi.Operation.Validation.Model;
using MovieStoreApi.Services.Logger;
using Newtonsoft.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerService _loggerService;

    public ExceptionMiddleware(RequestDelegate next, ILoggerService loggerService)
    {
        _next = next;
        _loggerService = loggerService;
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
        catch (ValidationException ex) // Fluent Validation'dan gelen hataları yakala
        {
            await HandleValidationException(context, ex);
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
        string message = "";

        // If the error sent is CustomException, take action accordingly
        if (ex is CustomException customException)
        {
            context.Response.StatusCode = customException.ErrorCode; // ErrorCode
            error = customException.Error; //Error Message
            message = customException.Message;
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            error = "Internal Server Error";
            message = ex.Message;
        }

        string ErrorInfo = string.Format(
            "Error Time:{0}" +
            "\nHost:{1}" +
            "\nMethod:{2}" +
            "\nPath:{3}" +
            "\nStatus:{4}" +
            "\nError:{5}" +
            "\nMessage:{6}", DateTime.Now, context.Request.Host, context.Request.Method, context.Request.Path, context.Response.StatusCode, error, ex.Message
        );

        _loggerService.LogError(ErrorInfo);

        var response = new { status = context.Response.StatusCode, error, message = ex.Message };
        var jsonResponse = JsonConvert.SerializeObject(response);

        await context.Response.WriteAsync(jsonResponse);
    }

    private async Task HandleValidationException(HttpContext context, ValidationException ex)
    {
        context.Response.ContentType = "application/json";

        var validationErrors = ex.Errors
            .Select(error => new CustomValidationError
            {
                PropertyName = error.PropertyName,
                ErrorMessage = error.ErrorMessage
            })
            .ToList()
            .ToDictionary(error => error.PropertyName, error => new[] { error.ErrorMessage }); ;

        var response = new
        {
            status = StatusCodes.Status400BadRequest,
            message = "Validation Error",
            errors = validationErrors
        };

        string errorInfo = string.Format(
            "Error Time:{0}" +
            "\nHost:{1}" +
            "\nMethod:{2}" +
            "\nPath:{3}" +
            "\nStatus:{4}" +
            "\nMessage:{5}" +
            "\nError:{6}", DateTime.Now, context.Request.Host, context.Request.Method, context.Request.Path, response.status, response.message, JsonConvert.SerializeObject(response.errors)
        );
        _loggerService.LogError(errorInfo);

        var jsonResponse = JsonConvert.SerializeObject(response);
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
