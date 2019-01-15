using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class LoginVM
    {
        [Required, MaxLength(30)]
        public string Usuario { get; set; }

        [Required, DataType(DataType.Password), MinLength(6), MaxLength(20)]
        public string Senha { get; set; }

        public bool Lembrar { get; set; }

        public string ReturnUrl {get;set;}

    }
}