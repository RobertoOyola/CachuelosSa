using Entitys.CachuelosSA;
using Entitys.Entitys;

namespace Repositories.UsuarioRepo
{
    public interface IUsuariosRepository
    {
        Task<Usuario> ObtenerUserXId(int idUser);
        Task<UsuarioInfo> ObtenerUserInfoXId(int idUser);
        Task<UsuarioInfo> ActualizarUserInfoXId(UsuarioInfo userInfo);
        Task<Usuario> ActualizarUsuario(Usuario user);
        Task<Usuario> ObtenerUserXCorreo(string email);
    }
}
