using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using System.Net;

public class RateLimitFilter : IActionFilter
{
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);
    private readonly int _maxRequests = 8;

    public RateLimitFilter(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
        if (ip == null)
        {
            context.Result = new ContentResult
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Content = "Unable to determine IP address."
            };
            return;
        }

        var key = $"RateLimit:{ip}";
        var timeNow = DateTime.UtcNow;

        var entry = _cache.Get<(DateTime timestamp, int count)?>(key);

        if (entry == null || timeNow - entry.Value.timestamp >= _rateLimitWindow)
        {
            _cache.Set(key, (timeNow, 1), _rateLimitWindow);
        }
        else
        {
            var (timestamp, count) = entry.Value;

            if (count >= _maxRequests)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.TooManyRequests,
                    Content = "Too many requests, please try again later."
                };
                return;
            }

            _cache.Set(key, (timestamp, count + 1), _rateLimitWindow - (timeNow - timestamp));
        }
    }


    public void OnActionExecuted(ActionExecutedContext context) { }
}
