using Microsoft.AspNetCore.Builder;

namespace SeaBattle.GameResources.Utilites
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseErrorLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLoggerMiddleware>();
        }
    }
}
