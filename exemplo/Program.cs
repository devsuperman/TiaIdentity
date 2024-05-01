using Microsoft.EntityFrameworkCore;
using App.Services;
using App.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddTiaIdentity()
                .AddCookie(x =>
                {
                    x.LoginPath = "/autenticacao/login";
                    x.AccessDeniedPath = "/autenticacao/logout";
                    x.LogoutPath = "/autenticacao/logout";
                });

builder.Services.AddTransient<GeradorDeListas>();

builder.Services.AddDbContext<Contexto>(options =>
   options.UseSqlite(builder.Configuration.GetConnectionString("AppDB")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseTiaIdentity();            
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
