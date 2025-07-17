using System.Threading.Tasks;
using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Usuarios;
using Repositories.UsuarioRepo;
using Services.Auth;

namespace Services.UsersServi
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuariosRepository _usuRepo;
        private readonly IAuthService _authServ;
        public UsuarioService(IUsuariosRepository usuRepo, IAuthService authServ)
        {
            _usuRepo = usuRepo;
            _authServ = authServ;
        }

        public async Task<ServiceResult<UsuarioInfo>> CambiarFotoUsuario(UsuariosInfoDto usuariosInfoDto)
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            UsuarioInfo usuarioInfo = await _usuRepo.ObtenerUserInfoXId(usuario.Id);
            if (usuarioInfo == null) return ServiceResult<UsuarioInfo>.Fail("UsuarioInfo no Encontrado", 204);

            usuarioInfo.UrlImg = usuariosInfoDto.UrlImg;

            usuarioInfo = await _usuRepo.ActualizarUserInfoXId(usuarioInfo);
            if (usuarioInfo == null) return ServiceResult<UsuarioInfo>.Fail("Imagen no Actualizada", 409);

            return ServiceResult<UsuarioInfo>.Ok(usuarioInfo, "Imagen actualizada con Exito", 201);

        }

        public async Task<ServiceResult<UsuarioInfo>> CambiarDescripcionUsuario(UsuariosInfoDto usuariosInfoDto)
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            UsuarioInfo usuarioInfo = await _usuRepo.ObtenerUserInfoXId(usuario.Id);
            if (usuarioInfo == null) return ServiceResult<UsuarioInfo>.Fail("UsuarioInfo no Encontrado", 204);

            usuarioInfo.Descripcion = usuariosInfoDto.Descripcion;

            usuarioInfo = await _usuRepo.ActualizarUserInfoXId(usuarioInfo);
            if (usuarioInfo == null) return ServiceResult<UsuarioInfo>.Fail("Imagen no Actualizada", 409);

            return ServiceResult<UsuarioInfo>.Ok(usuarioInfo, "Imagen actualizada con Exito", 201);
        }

        public async Task<ServiceResult<UsuarioXInfoCompleta>> ObtenerInfoUsuario()
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            Usuario user = await _usuRepo.ObtenerUserXId(usuario.Id);
            if (user == null) return ServiceResult<UsuarioXInfoCompleta>.Fail("Usuario no Encontrado", 204);

            UsuarioInfo usuarioInfo = await _usuRepo.ObtenerUserInfoXId(usuario.Id);
            if (usuarioInfo == null) return ServiceResult<UsuarioXInfoCompleta>.Fail("UsuarioInfo no Encontrado", 204);

            UsuarioXInfoCompleta usuarioXInfoCompleta = MappingUsuarios.MapearInfoCompleta(user, usuarioInfo);
            if (usuarioInfo == null) return ServiceResult<UsuarioXInfoCompleta>.Fail("Error al traer la informacion", 409);

            return ServiceResult<UsuarioXInfoCompleta>.Ok(usuarioXInfoCompleta, "Informacion Obtenida con Exito", 200);

        }
    }
}
