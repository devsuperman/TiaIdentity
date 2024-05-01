using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    public class LoginVM
    {
        [Required, MaxLength(30)]
        public string Usuario { get; set; }

        public bool Lembrar { get; set; }

        public string ReturnUrl {get;set;}

    }
}