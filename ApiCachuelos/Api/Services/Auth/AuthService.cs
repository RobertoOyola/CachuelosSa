using Api.CachuelosSA;
using Api.Entitys;
using Api.Entitys.Auth;
using Api.Repositories.Auth;
using Api.Utils;
using Microsoft.Win32;

namespace Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepo, IConfiguration configuration)
        {
            _authRepo = authRepo;
            _configuration = configuration;
        }

        public async Task<ServiceResult<Usuario>> Login(Login login)
        {
            bool existeUsuario = await _authRepo.CorreoExiste(login.email);
            if (!existeUsuario) return ServiceResult<Usuario>.Fail("Correo no encontrado", 404);

            var key = _configuration["Security:PasswordKey"];
            if (string.IsNullOrEmpty(key))
                return ServiceResult<Usuario>.Fail("Error al encriptar Contacte al Servicio Cliente", 403);

            login.password = Encript.EncriptarContra(login.password, key);

            Usuario newUser = await _authRepo.LoginUser(login);

            if (newUser != null)
            {
                if (newUser.Activo == false)
                    return ServiceResult<Usuario>.Fail("Usuario desactivado o bloqueado", 401);

                return ServiceResult<Usuario>.Ok(newUser, "Login con Exito", 200);
            }
            else
            {
                return ServiceResult<Usuario>.Fail("Contrasenia Erronea", 404);
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
                return ServiceResult<Usuario>.Ok(newUser, "Usuario Creado con Exito", 201);
            }
            else
            {
                return ServiceResult<Usuario>.Fail("Error al crear", 401);
            }

        }
    }
}
