using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class Usuario
	{		
        public int Id { get; set; }
        public Usuario()
        {
            
        }

		public Usuario(string nome, string email, string perfil)
		{
            Nome = nome;
            Email = email;
            Perfil = perfil;
		}
		
        public string Login { get => Nome; }

        [Required, MaxLength(50)]
        public string Nome { get; set; }

		[Required, MaxLength(50), EmailAddress]
		public string Email { get; set; }

		public string Hash {get; set; }

		public bool HashUtilizado {get;set;}

        public string Perfil { get; set; }

        public void Atualizar(string nome, string email, string perfil)
        {
            this.Nome=nome;
            this.Email = email;
            this.Perfil = perfil;
        }
    }
}