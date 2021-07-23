using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TiaIdentity
{
    public class Autenticador
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public Autenticador(IHttpContextAccessor _httpContextAccessor)
        {
            this.httpContextAccessor = _httpContextAccessor;
        }

        public async Task LoginAsync(IUsuario usuario, bool lembrar)
        {
            await LoginAsync(usuario.Login, usuario.Nome, lembrar, usuario.Perfil);
        }

        public async Task LoginAsync(string login, string nome, bool lembrar, string perfil, List<Claim> outrasClaims = null)
        {
            var perfis = new List<string> { perfil };
            await LoginAsync(login, nome, lembrar, perfis, outrasClaims);
        }

        public async Task LoginAsync(string login, string nome, bool lembrar, List<string> perfis, List<Claim> outrasClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nome),
                new Claim(ClaimTypes.NameIdentifier, login)
            };

            if (perfis != null)
                perfis.ForEach(f => claims.Add(new Claim(ClaimTypes.Role, f)));

            if (outrasClaims != null)
                outrasClaims.ForEach(f => claims.Add(f));

            await LogarComCookies(lembrar, claims);
        }

        private async Task LogarComCookies(bool lembrar, List<Claim> claims)
        {
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties { IsPersistent = lembrar };

            await httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimPrincipal,
                authProperties);
        }

        public async Task LogoutAsync()
        {
            await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }



    }

}