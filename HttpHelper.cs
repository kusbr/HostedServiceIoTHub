using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace System.Web
{
    public static class HttpContext
    {
        private static IHttpContextAccessor _contextAccessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;

        internal static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
    }
    
}

namespace D2C
{
    public static class StaticHttpContextExtensions
     {
          public static void AddHttpContextAccessor(this IServiceCollection services)
          {
               services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
          }

          public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
          {
               var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
               System.Web.HttpContext.Configure(httpContextAccessor);
               return app;
          }
     }
}