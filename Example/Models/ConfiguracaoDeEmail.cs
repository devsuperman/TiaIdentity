using System;

namespace App.Models
{
    public class ConfiguracaoDeEmail
    {
        public String Dominio { get; set; }
        public int Porta { get; set; }
        public bool SSL { get; set; }
        public String NomeDoRemetente { get; set; }
        public String EmailDoRemetente { get; set; }
        public String Senha { get; set; }               
    }
} 