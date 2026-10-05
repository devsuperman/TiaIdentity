using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TiaIdentity
{
    public sealed class Autenticador
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Autenticador(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task LoginAsync(IUsuario usuario, bool lembrar)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            await LoginAsync(
                usuario.Login,
                usuario.Nome,
                lembrar,
                new List<string> { usuario.Perfil });
        }

        public async Task LoginAsync(
            string login,
            string nome,
            bool lembrar,
            string perfil,
            List<Claim>? outrasClaims = null)
        {
            await LoginAsync(
                login,
                nome,
                lembrar,
                new List<string> { perfil },
                outrasClaims);
        }

        public async Task LoginAsync(
            string login,
            string nome,
            bool lembrar,
            List<string>? perfis,
            List<Claim>? outrasClaims = null)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Informe o login.", nameof(login));

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Informe o nome.", nameof(nome));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, login),
                new Claim(ClaimTypes.Name, nome)
            };

            if (perfis != null && perfis.Any())
            {
                claims.AddRange(
                    perfis
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Select(p => new Claim(ClaimTypes.Role, p)));
            }

            if (outrasClaims != null && outrasClaims.Any())
            {
                claims.AddRange(outrasClaims);
            }

            await EfetuarLoginAsync(lembrar, claims);
        }

        private async Task EfetuarLoginAsync(
            bool lembrar,
            IEnumerable<Claim> claims)
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HttpContext não disponível.");

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = lembrar,
                AllowRefresh = true,
                IssuedUtc = DateTimeOffset.UtcNow
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                properties);
        }

        public async Task LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HttpContext não disponível.");

            await httpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public string? LoginUsuario
        {
            get
            {
                return _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;
            }
        }

        public string? NomeUsuario
        {
            get
            {
                return _httpContextAccessor.HttpContext?
                    .User?
                    .Identity?
                    .Name;
            }
        }

        public bool EstaAutenticado
        {
            get
            {
                return _httpContextAccessor.HttpContext?
                    .User?
                    .Identity?
                    .IsAuthenticated ?? false;
            }
        }

        public List<string> Perfis
        {
            get
            {
                return _httpContextAccessor.HttpContext?
                    .User?
                    .FindAll(ClaimTypes.Role)
                    .Select(x => x.Value)
                    .ToList()
                    ?? new List<string>();
            }
        }

        public string? ObterClaim(string tipo)
        {
            return _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(tipo)?
                .Value;
        }

        public bool PossuiPerfil(string perfil)
        {
            return _httpContextAccessor.HttpContext?
                .User?
                .IsInRole(perfil) ?? false;
        }
    }
}
