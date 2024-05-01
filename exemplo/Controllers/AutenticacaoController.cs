using App.Models;
using App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers;

public class AutenticacaoController : Controller
{
    private readonly Contexto db;
    private readonly TiaIdentity.Autenticador tiaIdentity;

    public AutenticacaoController(Contexto db, TiaIdentity.Autenticador tiaIdentity)
    {
        this.db = db;
        this.tiaIdentity = tiaIdentity;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM viewmodel)
    {
        var usuario = await db.Usuarios.FirstOrDefaultAsync(a => a.Email == viewmodel.Usuario);

        var loginOuSenhaIncorretos = usuario == null;

        if (loginOuSenhaIncorretos)
            ModelState.AddModelError("", "Usuário ou Senha incorretos!");

        if (ModelState.IsValid)
        {
            await tiaIdentity.LoginAsync(usuario.Email, usuario.Nome, viewmodel.Lembrar, usuario.Perfil);

            if (!string.IsNullOrEmpty(viewmodel.ReturnUrl))
                return Redirect(viewmodel.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await tiaIdentity.LogoutAsync();
        return View(nameof(Login));
    }

}