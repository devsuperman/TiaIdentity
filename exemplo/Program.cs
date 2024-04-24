using Microsoft.EntityFrameworkCore;
using App.Services;
using App.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddTiaIdentity()
                .AddCookie(x =>
                {
                    x.LoginPath = "/autenticacao/login";
                    x.AccessDeniedPath = "/autenticacao/acessonegado";
                });

builder.Services.AddTransient<GeradorDeListas>();
builder.Services.Configure<ConfiguracaoDeEmail>(builder.Configuration.GetSection("ConfiguracoesDeEmail"));
builder.Services.AddTransient<Email>();

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
app.UseAuthorization();
app.UseTiaIdentity();            

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
