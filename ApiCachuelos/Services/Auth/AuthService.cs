using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Auth;
using Entitys.Entitys.Mail;
using Entitys.Entitys.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.Auth;
using Repositories.Otp;
using Repositories.UsuarioRepo;
using Services.CatalogoSeri;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utils.Utilities;

namespace Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly IUsuariosRepository _userRepo;
        private readonly IOtpRepository _otpsRepo;
        private readonly ICatalogoService _cataServ;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthService(
            IAuthRepository authRepo,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IUsuariosRepository userRepo,
            ICatalogoService cataServ,
            IOtpRepository otpsRepo)
        {
            _authRepo = authRepo;
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
            _userRepo = userRepo;
            _cataServ = cataServ;
            _otpsRepo = otpsRepo;
        }

        public async Task<ServiceResult<Usuario>> Login(Login login)
        {
            bool existeUsuario = await _authRepo.CorreoExiste(login.email);
            if (!existeUsuario) return ServiceResult<Usuario>.Fail("Correo no encontrado", 204);

            var key = _authRepo.ObtenerHashKey();
            if (string.IsNullOrEmpty(key))
                return ServiceResult<Usuario>.Fail("Error al encriptar Contacte al Servicio Cliente", 403);

            login.password = Encript.EncriptarContra(login.password, key);

            Usuario newUser = await _authRepo.LoginUser(login);

            if (newUser != null)
            {
                newUser.ContrasenaHash = string.Empty;
                if (newUser.Activo == false)
                    return ServiceResult<Usuario>.Fail("Usuario desactivado o bloqueado", 401);

                if (newUser.Verificado == false)
                    return ServiceResult<Usuario>.Fail("Usuario no verificado", 206);

                return ServiceResult<Usuario>.Ok(newUser, "Login con Exito", 200);
            }
            else
            {
                return ServiceResult<Usuario>.Fail("Contrasenia Erronea", 403);
            }
        }

        public async Task<ServiceResult<Usuario>> Register(Register register)
        {
            bool existeUsuario = await _authRepo.UsuarioExiste(register.NombreUsuario);
            if (existeUsuario) return ServiceResult<Usuario>.Fail("Nombre de usuario ya utilizado", 409);

            bool existeCorreo = await _authRepo.CorreoExiste(register.Correo);
            if (existeCorreo) return ServiceResult<Usuario>.Fail("Correo ya utilizado", 409);

            var key = _configuration["Security:PasswordKey"];
            if (string.IsNullOrEmpty(key))
                return ServiceResult<Usuario>.Fail("Error al encriptar Contacte al Servicio Cliente", 403);

            string contrasenaHash = Encript.EncriptarContra(register.ContrasenaHash, key);
            register.ContrasenaHash = contrasenaHash;

            Usuario newUser = await _authRepo.InsertUser(register);

            if (newUser != null)
            {
                if (!await _authRepo.CreateUserInfo(newUser))
                    return ServiceResult<Usuario>.Fail("User Info", 401);

                return ServiceResult<Usuario>.Ok(newUser, "Usuario Creado con Exito", 201);
            }
            else
            {
                return ServiceResult<Usuario>.Fail("Error al crear", 401);
            }

        }

        public async Task<string> GenerarToken(Usuario user)
        {
            UsuarioInfo userinfo = await _userRepo.ObtenerUserInfoXId(user.Id);

            string imgUrl = string.Empty;
            if (userinfo != null && userinfo.UrlImg != null) imgUrl = userinfo.UrlImg; 

            Claim[] claims = new Claim[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("UserName", user.NombreUsuario),
                new Claim("ImgPerfil", imgUrl),
                new Claim("RodId", user.RolId.ToString()),
                new Claim("EsSubscriptor", user.Subscrito.ToString()),
                new Claim("EsVerificado", user.Verificado.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Usuarios OtenerTokenInfo()
        {
            try
            {
                var user = _contextAccessor.HttpContext?.User;

                if (user == null) return null;

                Usuarios infoUsuario = new Usuarios()
                {
                    Id = int.Parse(user.FindFirst(Const.Token.Id)?.Value),
                    NombreUsuario = user.FindFirst(Const.Token.Usuario)?.Value,
                    RolId = user.FindFirst(Const.Token.Rol)?.Value,
                    EsSuscriptor = user.FindFirst(Const.Token.EsSuscriptor)?.Value
                };

                return infoUsuario;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ServiceResult<MailReturn>> EnviarCorreoOtp(MailInfo mailInfo, string tipoOtp)
        {
            var key = _authRepo.ObtenerHashKey();
            var emailPw = _authRepo.ObtenerMailKey();

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(emailPw))
                return ServiceResult<MailReturn>.Fail("Error al obtener SecurityKeys Contacte al Servicio Cliente", 403);

            Usuario user = await _userRepo.ObtenerUserXCorreo(mailInfo.Mail);
            if (user == null)
                return ServiceResult<MailReturn>.Fail("Usuario desactivado, bloqueado o no existe", 401);

            bool verificarAntiguas = await _otpsRepo.EliminarOtpsNoUsadas(user.Id, tipoOtp);

            string otp = Encript.CrearOtp();

            OtpAction otpAction = await _otpsRepo.CrearOtp(user.Id, otp, tipoOtp, 10);
            if (otpAction == null)
                return ServiceResult<MailReturn>.Fail("Error al crear la otp", 401);

            SmtpConfig smtpConfig = await _cataServ.GetSmtpInfo();

            string htmlBody = Mail.OtpMail(otp);

            MailReturn mailReturn = await Mail.SendEmailAsync(mailInfo.Mail, "Clave Temporal", htmlBody, smtpConfig);

            if (mailReturn.Ok == true)
            {
                return ServiceResult<MailReturn>.Ok(mailReturn, "Correo Enviado con Exito", 200);
            }
            else
            {
                return ServiceResult<MailReturn>.Fail(mailReturn.Message, 403);
            }

        }

        public async Task<ServiceResult<Usuario>> VerificarOtp(string otp)
        {
            string mensaje = string.Empty;

            if (string.IsNullOrEmpty(otp))
                return ServiceResult<Usuario>.Fail("OTP no puede ser nulo o vacio", 400);
            
            OtpAction otpAction = await _otpsRepo.ObtenerOtpXOtp(otp);
            if (otpAction == null )
                return ServiceResult<Usuario>.Fail("Otp incorrecto", 404);
            if (otpAction.Expiracion < DateTime.Now)
                return ServiceResult<Usuario>.Fail("OTP caducada", 400);

            Usuario user = await _userRepo.ObtenerUserXId(otpAction.IdUsuario);
            if (user == null)
                return ServiceResult<Usuario>.Fail("Usuario no Encontrado", 204);
            if (user.Activo == false)
                if (otpAction.TipoOtp != Const.OtpTipo.DesbloquearUsu)
                    return ServiceResult<Usuario>.Fail("Usuario Bloqueado", 204);

            if (otpAction.TipoOtp == Const.OtpTipo.VerificarUsu)
            {
                user.Verificado = true;
                mensaje = "Verificar al Usuario";

                user = await _userRepo.ActualizarUsuario(user);
                if (user == null)
                    return ServiceResult<Usuario>.Fail($"Falla al Verificar el Usuario", 404);
            }
            else if (otpAction.TipoOtp == Const.OtpTipo.EliminarUsu)
            {
                user.Activo = false;
                mensaje = "Eliminar al Usuario";

                user = await _userRepo.ActualizarUsuario(user);
                if (user == null)
                    return ServiceResult<Usuario>.Fail($"Falla al Eliminar el Usuario", 404);
            }
            else if (otpAction.TipoOtp == Const.OtpTipo.DesbloquearUsu)
            {
                user.Activo = true;
                mensaje = "Desbloquear al Usuario";

                user = await _userRepo.ActualizarUsuario(user);
                if (user == null)
                    return ServiceResult<Usuario>.Fail($"Falla al Desbloquear el Usuario", 404);
            }
            else
            {
                user = await _userRepo.ActualizarUsuario(user);
                mensaje = "Validar Otp";
            }

            otpAction = await _otpsRepo.UsarOtp(otpAction);
            if (otpAction == null)
                return ServiceResult<Usuario>.Fail("Falla al verificar la Otp", 404);

            if (user != null)
                return ServiceResult<Usuario>.Ok(user, $"Exito al {mensaje}", 200);
            else
                return ServiceResult<Usuario>.Fail($"Error al {mensaje}", 500);
        }

        public async Task<ServiceResult<string>> CambiarContrasena(RecuperarContrasena recuperar)
        {
            string key = _authRepo.ObtenerHashKey();

            Usuario user = await _userRepo.ObtenerUserXId(recuperar.Id);
            if (user == null) return ServiceResult<string>.Fail("Usuario desactivado o bloqueado o no existe", 401);
            if (user.Correo != recuperar.Mail)
                return ServiceResult<string>.Fail("Correo Equivocado", 409);
            if (user.ContrasenaHash == Encript.EncriptarContra(recuperar.Password, key)) 
                return ServiceResult<string>.Fail("Contraseña usada previamente", 409);

            user.ContrasenaHash = Encript.EncriptarContra(recuperar.Password, key);

            user = await _userRepo.ActualizarUsuario(user);
            if (user != null)
                return ServiceResult<string>.Ok("Contraseña cambiada con exito", $"Exito al Cambiar la contraseña", 200);
            else
                return ServiceResult<string>.Fail($"Error al Cambiar la contraseña", 500);
        }
    }
}
