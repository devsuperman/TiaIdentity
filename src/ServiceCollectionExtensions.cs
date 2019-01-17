using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using TiaIdentity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
       public static AuthenticationBuilder AddTiaIdentity(this IServiceCollection services)
       {
            services.AddHttpContextAccessor();
            services.AddTransient<Autenticador>();              
            return services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);                
       }       
    }
}