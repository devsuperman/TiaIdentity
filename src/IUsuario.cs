
namespace TiaIdentity
{
    public interface IUsuario
    {
        int Id { get; }
        string Nome { get; }
        string Email { get; }
        string Perfil { get; }
    }
}