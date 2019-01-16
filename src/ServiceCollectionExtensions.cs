using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace TiaIdentity
{
    public static class ServiceCollectionExtension
    {
       public static IServiceCollection AddTiaIdentity(this IServiceCollection services)
       {
            services.AddHttpContextAccessor();
            services.AddTransient<Autenticador>();
            return services;
       }

    }
}