using App.Models;
using System.Threading.Tasks;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using App.ViewModels;
using Microsoft.EntityFrameworkCore;
using TiaIdentity;

namespace App.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly Contexto db;
        private readonly Autenticador tiaIdentity;
        private readonly Email email;
        private readonly GeradorDeListas geradorDeListas;

        public UsuariosController(Contexto db, Autenticador tiaIdentity, Email email, GeradorDeListas geradorDeListas)
        {
            this.db = db;
            this.tiaIdentity = tiaIdentity;
            this.email = email;
            this.geradorDeListas = geradorDeListas;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await db.Usuarios
                .AsNoTracking()
                .ToListAsync();

            return View(usuarios);
        }

        
        public IActionResult Criar()
        {
            CarregarPerfis();
            return View();
        }       

        [HttpPost]
        public async Task<IActionResult> Criar(UsuarioVM viewModel)
        {
            var EmailJaExiste = await db.Usuarios.AnyAsync(a => a.Email == viewModel.Email);
            
            if(EmailJaExiste)
                ModelState.AddModelError("Email", "Email já foi cadastrado!");

            if (ModelState.IsValid)
            {   
                var usuario = new Usuario(viewModel.Nome, viewModel.Email, viewModel.Perfil);

                //TODO: Retirar esse método para utilizar em produção
                usuario.AlterarSenha("123456");

                await db.AddAsync(usuario);
                await db.SaveChangesAsync();
                
                //TODO: Descomentar se for utilizar
                // await email.EnviarEmailParaCriacaoDeSenha(usuario.Email, usuario.Hash);

                return RedirectToAction(nameof(Index));
            }

            CarregarPerfis();
            return View(viewModel);
        }


        public async Task<IActionResult> Editar(int id)
        {
            var usuario = await db.Usuarios.FindAsync(id);

            if (usuario == null)            
                NotFound();            

            var viewModel = new UsuarioVM(usuario);
            CarregarPerfis();

            return View(viewModel);
        }       

        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioVM model)
        {   
            var EmailJaExisteEmOutroUsuario = await db.Usuarios.AnyAsync(a =>
                a.Id != model.Id && 
                a.Email == model.Email);
            
            if(EmailJaExisteEmOutroUsuario)
                ModelState.AddModelError("Email", "Email já foi cadastrado!");

            if (ModelState.IsValid)
            {                
                var usuario = await db.Usuarios.FindAsync(model.Id);

                usuario.Atualizar(model.Nome, model.Email, model.Perfil);
                db.Update(usuario);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            CarregarPerfis();
            return View(model);
        }

        private void CarregarPerfis() => ViewBag.Perfil = geradorDeListas.Perfis();



    }
    
}