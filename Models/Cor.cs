using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class Cor
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }        
    }
}