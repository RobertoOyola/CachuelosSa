using Entitys.Entitys;
using Entitys.CachuelosSA;
using Entitys.Entitys.Usuarios;

namespace Services.UsersServi
{
    public interface IUsuarioService
    {
        Task<ServiceResult<UsuarioInfo>> CambiarFotoUsuario(string usuariosInfoDto);
        Task<ServiceResult<UsuarioxUsuarioInfo>> ObtenerUsuario();
        Task<ServiceResult<UsuarioxUsuarioInfo>> ObtenerUsuario(int id);
        Task<ServiceResult<UsuarioInfo>> ActualizarUsuario(UsuarioInfoDto usuarioInfoDto);
        Task<ServiceResult<bool>> TieneInfoUser();
    }
}
