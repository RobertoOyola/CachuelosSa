using Entitys.CachuelosSA;

namespace Entitys.Entitys.Usuarios
{
    public class MappingUsuarios
    {
        public static UsuarioXInfoCompleta MapearInfoCompleta(Usuario user, UsuarioInfo userInfo)
        {
            try
            {
                UsuarioXInfoCompleta usuarioXInfoCompleta = new UsuarioXInfoCompleta()
                {
                    NombreUsuario = user.NombreUsuario,
                    Correo = user.Correo,
                    Subscrito = user.Subscrito,
                    FechaUltimoLogin = user.FechaUltimoLogin,
                    UrlImg = userInfo.UrlImg,
                    Descripcion = userInfo.Descripcion,
                    FechaUltimaConexion = userInfo.FechaUltimaConexion,
                };

                return usuarioXInfoCompleta;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static UsuarioDto MapearUsuarioDto(Usuario user)
        {
            try
            {
                UsuarioDto usuarioXInfoCompleta = new UsuarioDto()
                {
                    Id = user.Id,
                    NombreUsuario = user.NombreUsuario,
                    Correo = user.Correo,
                    Verificado = user.Verificado,
                    Activo = user.Activo,
                    Subscrito = user.Subscrito,
                    FechaFinSubscrito = user.FechaFinSubscrito,
                    FechaCreacion = user.FechaCreacion,
                    FechaUltimoLogin = user.FechaUltimoLogin,
                    FechaActualizacion = user.FechaActualizacion,
                    RolId = user.RolId,
                    TokenRecuperacion = user.TokenRecuperacion,
                    ExpiracionToken = user.ExpiracionToken,
                };

                return usuarioXInfoCompleta;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
