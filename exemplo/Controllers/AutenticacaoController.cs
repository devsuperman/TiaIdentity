using App.Models;
using App.Services;
using App.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers
{
    public class AutenticacaoController : Controller
    {
        private readonly Contexto db;
        private readonly TiaIdentity.Autenticador tiaIdentity;        
        private readonly Email servicoDeEmail;

        public AutenticacaoController(Contexto db, TiaIdentity.Autenticador tiaIdentity, Email servicoDeEmail)
        {            
            this.db = db;
            this.servicoDeEmail = servicoDeEmail;
            this.tiaIdentity = tiaIdentity;            
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM viewmodel)
        {               
            var usuario = await db.Usuarios.FirstOrDefaultAsync(a => a.Nome == viewmodel.Usuario);
            
            var loginOuSenhaIncorretos = (usuario == null) || !(usuario.SenhaCorreta(viewmodel.Senha));
                
            if (loginOuSenhaIncorretos)            
                ModelState.AddModelError("", "Usuário ou Senha incorretos!");

            if (ModelState.IsValid)
            {                
                await tiaIdentity.LoginAsync(usuario, viewmodel.Lembrar);
                return RedirectToAction("Index","Home");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await tiaIdentity.LogoutAsync();
            return View(nameof(Login));
        }     

        public ActionResult EsqueciMinhaSenha()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EsqueciMinhaSenha(string email)
        {
            var usuario = await db.Usuarios.FirstOrDefaultAsync(x => x.Email == email);

            if (usuario is null)
                return NotFound();
            
            usuario.GerarNovaHash();

            db.Update(usuario);
            await db.SaveChangesAsync();

            //TODO: Descomentar se for utilizar
            // await servicoDeEmail.EnviarEmailParaTrocaDeSenha(usuario.Email, usuario.Hash);                

            return Ok();
        }

         public async Task<IActionResult> AlterarSenha(string id)
        {
            
            var usuario = await db.Usuarios.FirstOrDefaultAsync(x => x.Hash == id);

            if (usuario == null || usuario.HashUtilizado)
            {
                // Este link já foi utilizado
                return RedirectToAction(nameof(Login));
            }

            var viewModel = new AlterarSenhaVM(usuario);

            return View(viewModel);
           
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarSenha(AlterarSenhaVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = await db.Usuarios.FirstOrDefaultAsync(x => x.Hash == viewModel.Id);

                if (usuario == null || usuario.HashUtilizado)                
                    return RedirectToAction(nameof(Login));                
                
                usuario.AlterarSenha(viewModel.NovaSenha);
                usuario.UtilizarHash();
                
                db.Update(usuario);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Login));
            }

            return View(viewModel);
        }

        public IActionResult AcessoNegado() => View();

    }
    
}