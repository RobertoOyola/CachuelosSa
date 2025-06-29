using Api.CachuelosSA;

namespace Api.Repositories.UsuarioRepo
{
    public interface IUsuariosRepository
    {
        Task<Usuario> ObtenerUserXId(int idUser);
        Task<UsuarioInfo> ObtenerUserInfoXId(int idUser);
        Task<UsuarioInfo> ActualizarUserInfoXId(UsuarioInfo userInfo);
    }
}
