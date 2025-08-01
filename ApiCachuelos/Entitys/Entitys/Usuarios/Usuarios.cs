namespace Entitys.Entitys.Usuarios
{
    public class RecuperarContrasena
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
    }

    public class Usuarios
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string RolId { get; set; }
        public string EsSuscriptor { get; set; }
    }

    public class ActualizarFoto
    {
        public string Id { get; set; }
        public string UrlImg { get; set; }
    }

    public class UsuarioDto
    {
        public int Id { get; set; }

        public string NombreUsuario { get; set; }

        public string Correo { get; set; }

        public string ContrasenaHash { get; set; }

        public bool? Verificado { get; set; }

        public bool? Activo { get; set; }

        public bool? Subscrito { get; set; }

        public DateTime? FechaFinSubscrito { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaUltimoLogin { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public int? RolId { get; set; }

        public string TokenRecuperacion { get; set; }

        public DateTime? ExpiracionToken { get; set; }
    }

    public class UsuarioXInfoCompleta
    {
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public bool? Subscrito { get; set; }
        public DateTime? FechaUltimoLogin { get; set; }
        public string UrlImg { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaUltimaConexion { get; set; }
    }
}
