using Microsoft.AspNetCore.Mvc.Filters;

public class SecurityHeadersFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var headers = context.HttpContext.Response.Headers;

        headers["X-Content-Type-Options"] = "nosniff";
        headers["X-XSS-Protection"] = "1; mode=block";
        headers["X-Frame-Options"] = "DENY";
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
