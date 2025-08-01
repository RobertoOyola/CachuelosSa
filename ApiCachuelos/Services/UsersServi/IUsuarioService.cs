using Entitys.Entitys;
using Entitys.CachuelosSA;
using Entitys.Entitys.Usuarios;

namespace Services.UsersServi
{
    public interface IUsuarioService
    {
        Task<ServiceResult<UsuarioInfo>> CambiarFotoUsuario(string usuariosInfoDto);
        Task<ServiceResult<UsuarioXInfoCompleta>> ObtenerInfoUsuario();
    }
}
