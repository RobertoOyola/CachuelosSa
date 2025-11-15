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
                    Nombre = string.IsNullOrEmpty(userInfo.Nombre) ? "" : userInfo.Nombre,
                    Apellido = string.IsNullOrEmpty(userInfo.Apellido) ? "" : userInfo.Apellido,
                    TipoIdentificacion = string.IsNullOrEmpty(userInfo.TipoIdentificacion) ? "" : userInfo.TipoIdentificacion,
                    Identificacion = string.IsNullOrEmpty(userInfo.Identificacion) ? "" : userInfo.Identificacion,
                    EstadoCivil = string.IsNullOrEmpty(userInfo.EstadoCivil) ? "" : userInfo.EstadoCivil,
                    Direccion = string.IsNullOrEmpty(userInfo.Direccion) ? "" : userInfo.Direccion,
                    Telefono = string.IsNullOrEmpty(userInfo.Telefono) ? "" : userInfo.Telefono,
                    Ciudad = string.IsNullOrEmpty(userInfo.Ciudad) ? "" : userInfo.Ciudad,
                    Provincia = string.IsNullOrEmpty(userInfo.Provincia) ? "" : userInfo.Provincia,
                    Nacionalidad = string.IsNullOrEmpty(userInfo.Nacionalidad) ? "" : userInfo.Nacionalidad,
                    TipoDiscapacidad = string.IsNullOrEmpty(userInfo.TipoDiscapacidad) ? "" : userInfo.TipoDiscapacidad,
                    UrlImg = string.IsNullOrEmpty(userInfo.UrlImg) ? "" : userInfo.UrlImg,
                    Descripcion = string.IsNullOrEmpty(userInfo.Descripcion) ? "" : userInfo.Descripcion,
                    FechaNacimiento = userInfo.FechaNacimiento,
                    Discapacidad = userInfo.Discapacidad,
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
                userInfo.TipoDiscapacidad = string.IsNullOrEmpty(userInfoDto.TipoDiscapacidad) ? userInfo.TipoDiscapacidad : "";
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
