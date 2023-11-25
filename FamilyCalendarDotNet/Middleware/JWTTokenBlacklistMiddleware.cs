/*
 * This middleware will take the incoming request's Authorization header
 * and retrieve the token. It will then use the JWTTokenBlacklistingService
 * to check if the incoming authorization token is blacklisted. If the token
 * is blacklisted it will respond with a 401 status code (Unauthorized). 
 * Otherwise it will continue as normal.
 */
namespace FamilyCalendarDotNet;
public class JWTTokenBlacklistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JWTTokenBlacklistingService _blacklistService;

    public JWTTokenBlacklistMiddleware(RequestDelegate next, JWTTokenBlacklistingService blacklistService)
    {
        _next = next;
        _blacklistService = blacklistService;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        Console.WriteLine($"Checking token from middleware {token}");

        if (!string.IsNullOrEmpty(token) && _blacklistService.IsTokenBlacklisted(token))
        {
            Console.WriteLine($"{token} is blacklisted. Returning a 401 Unauthorized status code.");
            // Token is blacklisted, reject the request
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        Console.WriteLine("Extracting path from the request.");
        var path = context.Request.Path.Value;

        Console.WriteLine($"Extracted path {path}");

        Console.WriteLine("Extracting route from the path.");
        string pathRoute = path?.Split("/").Last<String>() ?? "";

        Console.WriteLine($"Extracted route from path {pathRoute}");

        Console.WriteLine("Checking if the route is the logout route");

        if(pathRoute == "Logout")
        {
            Console.WriteLine("The logout route has been called, blacklisting the token.");
            _blacklistService.AddToBlacklist(token!);
            Console.WriteLine($"Blacklisted the token {token}");
        }

        await _next(context);
    }
}

