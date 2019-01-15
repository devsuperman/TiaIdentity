using System;
using App.Models;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class AlterarSenhaVM
    {
        public AlterarSenhaVM()
        {

        }

        public AlterarSenhaVM(Usuario usuario)
        {
            this.Id = usuario.Hash;
        }        

        [Required, Display(Name = "Nova Senha"), MinLength(6), MaxLength(20)]
        public string NovaSenha { get; set; }

        [Required, Display(Name = "Confirmar Nova Senha"), MinLength(6), MaxLength(20), Compare("NovaSenha")]
        public string ConfirmarSenha { get; set; }
        
        public string Id { get; set; }
    }
}
