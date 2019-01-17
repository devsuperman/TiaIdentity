
namespace TiaIdentity
{
    public interface IUsuario
    {
        int Id { get; }
        string Nome { get; }
        string Login { get; }        
        string Perfil { get; }
    }
}