using Api.CachuelosSA;
using Api.Entitys.Auth;

namespace Api.Services.Auth
{
    public interface IAuthService
    {
        Task<Usuario> Register(Register register);
    }
}
