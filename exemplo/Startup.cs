using App.Models;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTiaIdentity()
                .AddCookie(x =>
                {
                    x.LoginPath = "/autenticacao/login";                  
                    x.AccessDeniedPath = "/autenticacao/acessonegado";                                
                });                    

            services.AddTransient<Services.GeradorDeListas>();            
            services.Configure<ConfiguracaoDeEmail>(Configuration.GetSection("ConfiguracoesDeEmail"));
            services.AddTransient<Email>();

             services.AddDbContext<Contexto>(options =>
                options.UseSqlite(Configuration.GetConnectionString("AppDB")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {            

            if (env.IsDevelopment())
            {                
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHsts();
            app.UseStaticFiles();
            app.UseTiaIdentity();            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

       
    }
}
