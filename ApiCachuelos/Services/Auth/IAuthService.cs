using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Auth;
using Entitys.Entitys.Mail;
using Entitys.Entitys.Usuarios;

namespace Services.Auth
{
    public interface IAuthService
    {
        Task<ServiceResult<UsuarioDto>> Login(Login login);
        Task<ServiceResult<Usuario>> Register(Register register);
        Task<string> GenerarToken(int id);
        Usuarios OtenerTokenInfo();
        Task<ServiceResult<MailReturn>> EnviarCorreoOtp(MailInfo mailInfo, string TipoMail);
        Task<ServiceResult<Usuario>> VerificarOtp(string otp);
        Task<ServiceResult<string>> CambiarContrasena(RecuperarContrasena recuperar);
    }
}
