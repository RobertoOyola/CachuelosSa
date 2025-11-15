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
        public static UsuarioInfoDto MapearUsuarioInfoDto(UsuarioInfo userInfo)
        {
            try
            {
                UsuarioInfoDto usuarioXInfoCompleta = new UsuarioInfoDto()
                {
                    Nombre = userInfo.Nombre,
                    Apellido = userInfo.Apellido,
                    FechaNacimiento = userInfo.FechaNacimiento,
                    TipoIdentificacion = userInfo.TipoIdentificacion,
                    Identificacion = userInfo.Identificacion,
                    EstadoCivil = userInfo.EstadoCivil,
                    Direccion = userInfo.Direccion,
                    Telefono = userInfo.Telefono,
                    Ciudad = userInfo.Ciudad,
                    Provincia = userInfo.Provincia,
                    Nacionalidad = userInfo.Nacionalidad,
                    Discapacidad = userInfo.Discapacidad,
                    TipoDiscapacidad = userInfo.TipoDiscapacidad,
                    UrlImg = userInfo.UrlImg,
                    Descripcion = userInfo.Descripcion,
                };

                return usuarioXInfoCompleta;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static UsuarioInfo MapearUsuarioInfoDto(UsuarioInfo userInfo, UsuarioInfoDto userInfoDto)
        {
            try
            {
                userInfo.Nombre = string.IsNullOrEmpty(userInfoDto.Nombre) ? userInfo.Nombre : userInfoDto.Nombre;
                userInfo.Apellido = string.IsNullOrEmpty(userInfoDto.Apellido) ? userInfo.Apellido : userInfoDto.Apellido;
                userInfo.FechaNacimiento = userInfoDto.FechaNacimiento ?? userInfo.FechaNacimiento;
                userInfo.TipoIdentificacion = string.IsNullOrEmpty(userInfoDto.TipoIdentificacion) ? userInfo.TipoIdentificacion : userInfoDto.TipoIdentificacion;
                userInfo.Identificacion = string.IsNullOrEmpty(userInfoDto.Identificacion) ? userInfo.Identificacion : userInfoDto.Identificacion;
                userInfo.EstadoCivil = string.IsNullOrEmpty(userInfoDto.EstadoCivil) ? userInfo.EstadoCivil : userInfoDto.EstadoCivil;
                userInfo.Direccion = string.IsNullOrEmpty(userInfoDto.Direccion) ? userInfo.Direccion : userInfoDto.Direccion;
                userInfo.Telefono = string.IsNullOrEmpty(userInfoDto.Telefono) ? userInfo.Telefono : userInfoDto.Telefono;
                userInfo.Ciudad = string.IsNullOrEmpty(userInfoDto.Ciudad) ? userInfo.Ciudad : userInfoDto.Ciudad;
                userInfo.Provincia = string.IsNullOrEmpty(userInfoDto.Provincia) ? userInfo.Provincia : userInfoDto.Provincia;
                userInfo.Nacionalidad = string.IsNullOrEmpty(userInfoDto.Nacionalidad) ? userInfo.Nacionalidad : userInfoDto.Nacionalidad;
                userInfo.Discapacidad = userInfoDto.Discapacidad ?? userInfo.Discapacidad;
                userInfo.TipoDiscapacidad = string.IsNullOrEmpty(userInfoDto.TipoDiscapacidad) ? userInfo.TipoDiscapacidad : userInfoDto.TipoDiscapacidad;
                userInfo.Descripcion = string.IsNullOrEmpty(userInfoDto.Descripcion) ? userInfo.Descripcion : userInfoDto.Descripcion;

                return userInfo;
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
