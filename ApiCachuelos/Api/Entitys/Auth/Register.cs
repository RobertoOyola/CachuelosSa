namespace Api.Entitys.Auth
{
    public class Register
    {
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string ContrasenaHash { get; set; }
    }
}
