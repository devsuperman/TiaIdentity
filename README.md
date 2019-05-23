# TiaIdentity: Autenticação, Autorização e Simplicidade

## Manual de Utilização
1. Instale o nuget package https://www.nuget.org/packages/TiaIdentity/
2. Chame o método services.AddTiaIdentity() e app.UseTiaIdentity() no Startup do projeto ([Veja o Startup no projeto de Exemplo](https://github.com/hsbtiago/TiaIdentity/blob/master/exemplo/Startup.cs))  
3. Configure os caminhos da página de login, acesso negado e etc com o .AddCookie() ([Veja o Startup no projeto de Exemplo](https://github.com/hsbtiago/TiaIdentity/blob/master/exemplo/Startup.cs))
4. Receba a dependência do TiaIdentity.Autenticador na controller ([Veja AutenticacaoController no projeto de Exemplo](https://github.com/hsbtiago/TiaIdentity/blob/master/exemplo/Controllers/AutenticacaoController.cs))  
5. Utilize os métodos LoginAsync e LogoutAsync conforme precisar ([Veja AutenticacaoController no projeto de Exemplo](https://github.com/hsbtiago/TiaIdentity/blob/master/exemplo/Controllers/AutenticacaoController.cs))

Bônus. O método LoginAsync tem 2 sobrecargas, uma com os parametros soltos e outra com a implementação da interface TiaIdentity.IUsuario.

### Extras
Para interar a autenticação, o projeto de exemplo contém também um método de criptografia de senha na classe usuário e uma classe para envio de email. Para ativar a classe de envio de email faça o seguinte:
1. Digite as credenciais de email no appsettings.json  
2. Descomente a linha de envio de Email em UsuariosController.Criar  
3. Descomente a linha de envio de Email em AutenticacaoController.EsqueciMinhaSenha  
