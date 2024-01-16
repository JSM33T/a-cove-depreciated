using almondcove.Middlewares;

namespace almondcove.Extensions
{
    public static class CookieCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseCookieCheckMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<SessionManager>();
    }
}
