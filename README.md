# TiaIdentity
## Autenticação, Autorização e Simplicidade

* Rotas úteis para testes
Criação de Usuário (acesso público): /Usuarios/Criar
Início (acesso somente para usuário logado): /
Cores (acesso somente para usuário com perfil de Administrador): /Cores

* Envio de E-mail para criação e alteração de senha
Para habilitar o envio de email, realize os seguintes passos:
1- Digite as credenciais de email no appsettings.json
2- Descomente a linha de envio de Email em UsuariosController.Criar
2- Descomente a linha de envio de Email em AutenticacaoController.EsqueciMinhaSenha