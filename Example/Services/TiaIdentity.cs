using System;
using App.Models;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace App.Services
{
    public class TiaIdentity
    {
        private readonly IHttpContextAccessor httpContextAccessor;        

        public TiaIdentity(IHttpContextAccessor _httpContextAccessor)
        {
            this.httpContextAccessor = _httpContextAccessor;                        
        }

        internal async Task LoginAsync(Usuario usuario, bool lembrar)
        {
            var claims = new List<Claim>
                {   
                    new Claim(ClaimTypes.Name, usuario.Nome),                                     
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Perfil)                                                        
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(5),
                IsPersistent = lembrar                
            };
            
            await httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }


        internal bool SenhaCorreta(string senhaDigitada, string senhaSalva)
        {   
            var senhaDigitadaCriptografada = CriptografarSenha(senhaDigitada);
            return (senhaSalva == senhaDigitadaCriptografada);
        }


        internal async Task LogoutAsync()
        {
            await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        } 

        public string CriptografarSenha(string txt)
        {            
            var algoritmo = SHA512.Create();
            var senhaEmBytes = Encoding.UTF8.GetBytes(txt);
            var senhaCifrada = algoritmo.ComputeHash(senhaEmBytes);
            
            var sb = new StringBuilder();
            
            foreach (var caractere in senhaCifrada)            
                sb.Append(caractere.ToString("X2"));
            
            return sb.ToString();
        }

    }
}