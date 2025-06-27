using Api.CachuelosSA;
using Api.Entitys.Auth;
using Api.Repositories.Auth;
using Api.Utils;

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

        public async Task<Usuario> Register(Register register)
        {
            bool existe = await _authRepo.UsuarioExiste(register.NombreUsuario);
            if (existe) return null;

            var key = _configuration["Security:PasswordKey"];

            if (string.IsNullOrEmpty(key))
                return null;

            var contrasenaHash = Encript.EncriptarContra(register.ContrasenaHash, key);

            register.ContrasenaHash = contrasenaHash;

            Usuario newUser = await _authRepo.InsertUser(register);

            return newUser;

        }
    }
}
