using Entitys.CachuelosSA;
using Entitys.Entitys.Auth;

namespace Repositories.Auth
{
    public interface IAuthRepository
    {

        Task<bool> UsuarioExiste(string nombreUsuario);
        Task<bool> CorreoExiste(string nombreUsuario);
        Task<Usuario> InsertUser(Register register);
        Task<Usuario> LoginUser(Login login);
        Task<bool> CreateUserInfo(Usuario user);
        string ObtenerHashKey();
        string ObtenerMailKey();
        Task<Usuario> UsuarioXOtp(string otp);
    }
}
