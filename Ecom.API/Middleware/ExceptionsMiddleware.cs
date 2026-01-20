using Ecom.API.Helper;
using System.Net;
using System.Text.Json;

namespace Ecom.API.Middleware;

public class ExceptionsMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionsMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var response = new ApiExceptions((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace);
            var json = JsonSerializer.Serialize(response);
            context.Response.WriteAsync(json);
        }
    }
}
