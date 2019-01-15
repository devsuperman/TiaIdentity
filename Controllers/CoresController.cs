using System.Linq;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles= Perfis.Administrador)]
    public class CoresController : Controller
    {
        private readonly Contexto db;

        public CoresController(Contexto contexto) => db = contexto;

        public async Task<IActionResult> Index()
        {
            var lista = await db.Cores                         
                .AsNoTracking()
                .ToListAsync();
          
            return View(lista);
        }


        public async Task<IActionResult> Detalhes(int? id)
        {
            if (id == null)            
                return NotFound();            

            var cor = await db.Cores.SingleOrDefaultAsync(m => m.Id == id);
            
            if (cor == null)            
                return NotFound();            

            return View(cor);
        }

        
        public ActionResult Criar()
        {        
            return View();
        }

        

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(Cor cor)
        {
            if (ModelState.IsValid)
            {
                db.Add(cor);
                
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(cor);
        }
        
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)            
                return BadRequest();
            
            var cor = await db.Cores.SingleOrDefaultAsync(m => m.Id == id);
            
            if (cor == null)            
                return NotFound();
            
            return View(cor);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Cor cor)
        {           
            
            if (ModelState.IsValid)
            {
                
                db.Update(cor);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(cor);
        }            

    }
}