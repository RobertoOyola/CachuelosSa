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
    }
}
