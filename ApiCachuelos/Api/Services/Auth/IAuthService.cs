using Api.CachuelosSA;
using Api.Entitys;
using Api.Entitys.Auth;

namespace Api.Services.Auth
{
    public interface IAuthService
    {
        Task<ServiceResult<Usuario>> Login(Login login);
        Task<ServiceResult<Usuario>> Register(Register register);
    }
}
