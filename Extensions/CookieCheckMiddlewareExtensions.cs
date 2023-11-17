using almondCove.Middlewares;

namespace almondCove.Extensions
{
    public static class CookieCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseCookieCheckMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionManager>();
        }
    }
}
