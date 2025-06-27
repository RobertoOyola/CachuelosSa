using Api.CachuelosSA;
using Api.Entitys.Auth;

namespace Api.Repositories.Auth
{
    public interface IAuthRepository
    {

        Task<bool> UsuarioExiste(string nombreUsuario);
        Task<bool> CorreoExiste(string nombreUsuario);
        Task<Usuario> InsertUser(Register register);
        Task<Usuario> LoginUser(Login login);
    }
}
