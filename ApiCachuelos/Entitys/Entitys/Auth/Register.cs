namespace Entitys.Entitys.Auth
{
    public class Register
    {
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string ContrasenaHash { get; set; }
    }

    public class SaveOtp
    {
        public string Otp { get; set; }
        public DateTime HoraCaducidad { get; set; }
    }
}
