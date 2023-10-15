using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MovieStoreApi.Services.Logger;
using System;
using System.Text;
using System.Threading.Tasks;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerService _loggerService;

    public LoggerMiddleware(RequestDelegate next, ILoggerService loggerService)
    {
        _next = next;
        _loggerService = loggerService;
    }

    public async Task Invoke(HttpContext context)
    {
        string requestBody = "";
        if (context.Request.ContentLength > 0)
        {
            context.Request.EnableBuffering(); // Allow reading the body multiple times

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Seek(0, SeekOrigin.Begin); // Reset the request body position for other middlewares
            }
        }

        string requestInfo = string.Format(
            "Request Time:{0}\n" +
            "Host:{1}\n" +
            "Method:{2}\n" +
            "Path:{3}\n" +
            "QueryString:{4}\n" +
            "Request Body:{5}", DateTime.Now, context.Request.Host, context.Request.Method, context.Request.Path, context.Request.QueryString, requestBody
        );

        _loggerService.LogRequest(requestInfo);

        var originalBodyStream = context.Response.Body;
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            try
            {
                // Continue processing the request
                await _next(context);

                // Log response body if it is available
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBodyContent = await new StreamReader(context.Response.Body).ReadToEndAsync();

                // Copy the captured response body back to the original stream
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);

                string responseInfo = string.Format(
                    "Response Time:{0}\n" +
                    "Host:{1}\n" +
                    "Method:{2}\n" +
                    "Path:{3}\n" +
                    "StatusCode:{4}\n" +
                    "QueryString:{5}\n" +
                    "Response Body:{6}", DateTime.Now, context.Request.Host, context.Request.Method, context.Request.Path, context.Response.StatusCode, context.Request.QueryString, responseBodyContent
                );

                _loggerService.LogResponse(responseInfo);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }
    }
}

public static class LoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggerMiddleware>();
    }
}
