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

        public async Task<ServiceResult<UsuarioInfo>> CambiarFotoUsuario(string IdFoto)
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            UsuarioInfo usuarioInfo = await _usuRepo.ObtenerUserInfoXIdUser(usuario.Id);
            if (usuarioInfo == null) return ServiceResult<UsuarioInfo>.Fail("UsuarioInfo no Encontrado", 204);

            usuarioInfo.UrlImg = IdFoto;

            usuarioInfo = await _usuRepo.ActualizarUserInfoXId(usuarioInfo);
            if (usuarioInfo == null) return ServiceResult<UsuarioInfo>.Fail("Imagen no Actualizada", 409);

            return ServiceResult<UsuarioInfo>.Ok(usuarioInfo, "Imagen actualizada con Exito", 201);

        }

        public async Task<ServiceResult<UsuarioxUsuarioInfo>> ObtenerUsuario()
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            Usuario user = await _usuRepo.ObtenerUserXId(usuario.Id);
            if (user == null) return ServiceResult<UsuarioxUsuarioInfo>.Fail("Usuario no Encontrado", 204);

            UsuarioInfo usuarioInfo = await _usuRepo.ObtenerUserInfoXIdUser(usuario.Id);
            if (usuarioInfo == null) return ServiceResult<UsuarioxUsuarioInfo>.Fail("UsuarioInfo no Encontrado", 204);

            UsuarioDto usuarioDto = MappingUsuarios.MapearUsuarioDto(user);
            UsuarioInfoDto usuarioInfoDto = MappingUsuarios.MapearUsuarioInfoDto(usuarioInfo);
            if (usuarioDto == null || usuarioInfoDto == null) return ServiceResult<UsuarioxUsuarioInfo>.Fail("Error al formatear la informacion", 204);

            UsuarioxUsuarioInfo response = new UsuarioxUsuarioInfo()
            {
                UsuarioDto = usuarioDto,
                UsuarioInfoDto = usuarioInfoDto
            };

            return ServiceResult<UsuarioxUsuarioInfo>.Ok(response, "Informacion Obtenida con Exito", 200);
        }

        public async Task<ServiceResult<UsuarioxUsuarioInfo>> ObtenerUsuario(int id)
        {
            Usuario user = await _usuRepo.ObtenerUserXId(id);
            if (user == null) return ServiceResult<UsuarioxUsuarioInfo>.Fail("Usuario no Encontrado", 204);

            UsuarioInfo usuarioInfo = await _usuRepo.ObtenerUserInfoXIdUser(id);
            if (usuarioInfo == null) return ServiceResult<UsuarioxUsuarioInfo>.Fail("UsuarioInfo no Encontrado", 204);

            UsuarioDto usuarioDto = MappingUsuarios.MapearUsuarioDto(user);
            UsuarioInfoDto usuarioInfoDto = MappingUsuarios.MapearUsuarioInfoDto(usuarioInfo);
            if (usuarioDto == null || usuarioInfoDto == null) return ServiceResult<UsuarioxUsuarioInfo>.Fail("Error al formatear la informacion", 204);

            UsuarioxUsuarioInfo response = new UsuarioxUsuarioInfo()
            {
                UsuarioDto = usuarioDto,
                UsuarioInfoDto = usuarioInfoDto
            };

            return ServiceResult<UsuarioxUsuarioInfo>.Ok(response, "Informacion Obtenida con Exito", 200);
        }

        public async Task<ServiceResult<UsuarioInfo>> ActualizarUsuario(UsuarioInfoDto usuarioInfoDto)
        {
            Usuarios userToken = _authServ.OtenerTokenInfo();

            Usuario user = await _usuRepo.ObtenerUserXId(userToken.Id);
            if (user == null) return ServiceResult<UsuarioInfo>.Fail("Usuario no Encontrado", 204);

            UsuarioInfo usuarioInfo = await _usuRepo.ObtenerUserInfoXIdUser(userToken.Id);
            if (usuarioInfo == null) return ServiceResult<UsuarioInfo>.Fail("UsuarioInfo no Encontrado", 204);

            usuarioInfo = MappingUsuarios.MapearUsuarioInfoDto(usuarioInfo, usuarioInfoDto);
            if (usuarioInfo == null) return ServiceResult<UsuarioInfo>.Fail("Error al pasar la informacion", 409);

            usuarioInfo = await _usuRepo.ActualizarUserInfoXId(usuarioInfo);
            if (usuarioInfo == null) return ServiceResult<UsuarioInfo>.Fail("Error al guardar la informacion", 409);

            usuarioInfo.IdUsuarioNavigation = null;
            return ServiceResult<UsuarioInfo>.Ok(usuarioInfo, "Informacion Actualizada con exito", 202);

        }
    }
}
