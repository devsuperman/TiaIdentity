using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using App.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace App.Services
{
    public class Email
    {
       private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ConfiguracaoDeEmail configuracoesDeEmail;

        public Email(IOptions<ConfiguracaoDeEmail> configuracoesDeEmail, IHttpContextAccessor httpContextAccessor)
        {
            this.configuracoesDeEmail = configuracoesDeEmail.Value;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task EnviarAsync(string destino, string assunto, string mensagem)
        {            
            var mail = new MailMessage()
            {
                From = new MailAddress(configuracoesDeEmail.EmailDoRemetente, configuracoesDeEmail.NomeDoRemetente),
                Subject = assunto,
                Body = mensagem,
                IsBodyHtml = true                
            };

            mail.To.Add(new MailAddress(destino));                        
            
            using (SmtpClient smtp = new SmtpClient(configuracoesDeEmail.Dominio, configuracoesDeEmail.Porta))
            {
                smtp.Credentials = new NetworkCredential(configuracoesDeEmail.EmailDoRemetente,configuracoesDeEmail.Senha);
                smtp.EnableSsl = configuracoesDeEmail.SSL;
                
                await smtp.SendMailAsync(mail);
            }
        }

        public async Task EnviarEmailParaCriacaoDeSenha(string emailDestino, string hashDeCriacaoDeSenha)
        {
            string titulo = "Criação de Senha";
            
            var mensagem = new StringBuilder();
            
            mensagem.Append("Acesse o seguinte link para criar sua senha: ");
            
            string linkTrocarSenha = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}/Autenticacao/AlterarSenha/{hashDeCriacaoDeSenha}";
            
            mensagem.Append(linkTrocarSenha);

            await this.EnviarAsync(emailDestino, titulo, mensagem.ToString());
        }

        public async Task EnviarEmailParaTrocaDeSenha(string emailDestino, string hashDeAlteracaoDeSenha)
        {
            string titulo = "Alteração de Senha";
            
            StringBuilder mensagem = new StringBuilder();
            mensagem.Append("Acesse o seguinte link para alterar sua senha: ");

            string linkTrocarSenha = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}/Autenticacao/AlterarSenha/{hashDeAlteracaoDeSenha}";            

            mensagem.Append(linkTrocarSenha);

            await this.EnviarAsync(emailDestino, titulo, mensagem.ToString());
        }  

    }
}