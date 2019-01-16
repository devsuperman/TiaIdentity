using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Cryptography;
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
            await LoginAsync(usuario.Login, usuario.Nome, usuario.Perfil, lembrar);
        }

        public async Task LoginAsync(string login, string nome, string perfil, bool lembrar)
        {
            var claims = new List<Claim>
                {   
                    new Claim(ClaimTypes.Name, nome),                                     
                    new Claim(ClaimTypes.NameIdentifier, login),
                    new Claim(ClaimTypes.Role, perfil)                                                        
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


        public bool SenhaCorreta(string senhaDigitada, string senhaSalva)
        {   
            var senhaDigitadaCriptografada = CriptografarSenha(senhaDigitada);
            return (senhaSalva == senhaDigitadaCriptografada);
        }


        public async Task LogoutAsync()
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