using Microsoft.EntityFrameworkCore;

namespace App.Models
{
    public class Contexto : DbContext
    {   
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }
                
        public DbSet<Cor> Cores {get;set;} 
        public DbSet<Usuario> Usuarios {get;set;}
    }
}