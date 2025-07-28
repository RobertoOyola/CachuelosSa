using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Auth;
using Entitys.Entitys.Mail;
using Entitys.Entitys.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.Auth;
using Repositories.UsuarioRepo;
using Services.CatalogoSeri;
using Utils.Utilities;

namespace Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly IUsuariosRepository _userRepo;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICatalogoService _cataServ;

        public AuthService(
            IAuthRepository authRepo, 
            IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor,
            IUsuariosRepository userRepo,
            ICatalogoService cataServ)
        {
            _authRepo = authRepo;
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
            _userRepo = userRepo;
            _cataServ = cataServ;
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
            newUser.ContrasenaHash = string.Empty;

            if (newUser != null)
            {
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

        public string GenerarToken(Usuario user)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(Const.TokenId, user.Id.ToString()),
                new Claim(Const.TokenUsuario, user.NombreUsuario),
                new Claim(Const.TokenRol, user.RolId.ToString()),
                new Claim(Const.TokenEsSuscriptor, user.Subscrito.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

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
                    Id = int.Parse(user.FindFirst(Const.TokenId)?.Value),
                    NombreUsuario = user.FindFirst(Const.TokenUsuario)?.Value,
                    RolId = user.FindFirst(Const.TokenRol)?.Value,
                    EsSuscriptor = user.FindFirst(Const.TokenEsSuscriptor)?.Value
                };

                return infoUsuario;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ServiceResult<MailReturn>> EnviarCorreoOtp(MailInfo mailInfo)
        {
            string otp = Encript.CrearOtp();

            var key = _authRepo.ObtenerHashKey();
            var emailPw = _authRepo.ObtenerMailKey();

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(emailPw))
                return ServiceResult<MailReturn>.Fail("Error al obtener SecurityKeys Contacte al Servicio Cliente", 403);

            Login userInfo = new Login
            {
                email = mailInfo.Mail,
                password = Encript.EncriptarContra(mailInfo.Password, key)
            };

            Usuario user = await _authRepo.LoginUser(userInfo);
            if (user == null)
                return ServiceResult<MailReturn>.Fail("Usuario desactivado o bloqueado", 401);

            user.TokenRecuperacion = otp;
            user.ExpiracionToken = DateTime.Now.AddMinutes(10);

            Usuario userOtp = await _userRepo.ActualizarUsuario(user);

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
    }
}
