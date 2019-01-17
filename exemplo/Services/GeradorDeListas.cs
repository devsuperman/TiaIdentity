using System.Linq;
using App.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace App.Services
{
    public class GeradorDeListas
    {
        private readonly Contexto db;

        public GeradorDeListas(Contexto db)
        {
            this.db = db;
        }

        public SelectList Perfis()
        {
            var lista = new List<string>();

            lista.Add(Models.Perfis.Administrador);
            lista.Add(Models.Perfis.Comum);            
            
            return new SelectList(lista);
        }        


        public async Task<SelectList> Cores()
        {
            var lista = await db.Cores                
                .Select(w => new {w.Id, w.Nome})
                .AsNoTracking()
                .ToListAsync();
            
            return new SelectList(lista, "Id", "Nome");
        }        
    }
}