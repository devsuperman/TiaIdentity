using System;
using System.ComponentModel.DataAnnotations;
using TiaIdentity;

namespace App.Models
{
    public class Usuario : IUsuario
	{		
        public int Id { get; set; }
        public Usuario()
        {
            
        }

		public Usuario(string nome, string email, string perfil)
		{
			GerarNovaHash();
            Nome = nome;
            Email = email;
            Perfil = perfil;
		}
		
        [Required, MaxLength(50)]
        public string Nome { get; set; }

		[Required, MaxLength(20)]
		public string Senha { get; set; } = Guid.NewGuid().ToString();

		[Required, MaxLength(50), EmailAddress]
		public string Email { get; set; }

		public string Hash {get; set; }

		public bool HashUtilizado {get;set;}

        public string Perfil { get; set; }

        public void AlterarSenha(string senhaCriptografada) => this.Senha = senhaCriptografada;

        public void UtilizarHash() => this.HashUtilizado = true;
        
        public void GerarNovaHash()
        {
            this.Hash = Guid.NewGuid().ToString();
            this.HashUtilizado = false;
        }


        public void Atualizar(string nome, string email, string perfil)
        {
            this.Nome=nome;
            this.Email = email;
            this.Perfil = perfil;
        }

    }
}