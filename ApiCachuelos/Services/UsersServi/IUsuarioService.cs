using Entitys.Entitys;
using Entitys.CachuelosSA;
using Entitys.Entitys.Usuarios;

namespace Services.UsersServi
{
    public interface IUsuarioService
    {
        Task<ServiceResult<UsuarioInfo>> CambiarFotoUsuario(UsuariosInfoDto usuariosInfoDto);
        Task<ServiceResult<UsuarioInfo>> CambiarDescripcionUsuario(UsuariosInfoDto usuariosInfoDto);
        Task<ServiceResult<UsuarioXInfoCompleta>> ObtenerInfoUsuario();
    }
}
