using Api.CachuelosSA;
using Api.Entitys.Usuarios;
using Api.Entitys;

namespace Api.Services.UsersServi
{
    public interface IUsuarioService
    {
        Task<ServiceResult<UsuarioInfo>> CambiarFotoUsuario(UsuariosInfoDto usuariosInfoDto);
        Task<ServiceResult<UsuarioInfo>> CambiarDescripcionUsuario(UsuariosInfoDto usuariosInfoDto);
    }
}
