namespace Api.Entitys.Usuarios
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string RolId { get; set; }
        public string EsSuscriptor { get; set; }
    }

    public class UsuariosInfoDto
    {
        public string UrlImg { get; set; }
        public string Descripcion { get; set; }
    }
}
