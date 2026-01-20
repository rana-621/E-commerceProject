using Ecom.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.API.Middleware;

public class ExceptionsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);

    public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment, IMemoryCache memoryCache)
    {
        _next = next;
        _hostEnvironment = hostEnvironment;
        _memoryCache = memoryCache;
    }
    public async Task InvokeAsync(HttpContext context)
    {

        try
        {

            if (IsRequestAllowed(context) == false)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.ContentType = "application/json";

                var response = new ApiExceptions((int)HttpStatusCode.TooManyRequests, "Too many requests. Please try again later.");

                await context.Response.WriteAsJsonAsync(response);

            }
            await _next(context);
        }

        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";


            var response = _hostEnvironment.IsDevelopment() ?
                new ApiExceptions((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                 : new ApiExceptions((int)HttpStatusCode.InternalServerError, ex.Message);

            var json = JsonSerializer.Serialize(response);
            context.Response.WriteAsync(json);
        }
    }

    public bool IsRequestAllowed(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress.ToString();
        var cachKey = $"Rate: {ip}";
        var dateNow = DateTime.Now;

        var (timesTamp, count) = _memoryCache.GetOrCreate(cachKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
            return (timesTamp: dateNow, count: 0);

        });

        if (dateNow - timesTamp < _rateLimitWindow)
        {
            if (count >= 8)
            {
                return false;
            }

            _memoryCache.Set(cachKey, (timesTamp, count += 1), _rateLimitWindow);

        }
        else
        {
            _memoryCache.Set(cachKey, (timesTamp, count), _rateLimitWindow);

        }
        return true;
    }

}
