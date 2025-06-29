using Api.CachuelosSA;
using Api.Entitys;
using Api.Entitys.Auth;
using Api.Entitys.Usuarios;

namespace Api.Services.Auth
{
    public interface IAuthService
    {
        Task<ServiceResult<Usuario>> Login(Login login);
        Task<ServiceResult<Usuario>> Register(Register register);
        string GenerarToken(Usuario user);
        Usuarios OtenerTokenInfo();
    }
}
