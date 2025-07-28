
using Entitys.CachuelosSA;
using Entitys.Entitys.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utils.Utilities;

namespace Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CachuelosSaContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(CachuelosSaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> UsuarioExiste( string nombreUsuario)
        {
            try
            {
                bool existe = await _context.Usuarios.AnyAsync(x => x.NombreUsuario == nombreUsuario);
                return existe;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CorreoExiste(string correo)
        {
            try
            {
                bool existe = await _context.Usuarios.AnyAsync(x => x.Correo == correo);
                return existe;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Usuario> InsertUser( Register register )
        {
            try
            {
                Usuario newUser = new Usuario()
                {
                    NombreUsuario = register.NombreUsuario,
                    Correo = register.Correo,
                    ContrasenaHash = register.ContrasenaHash,
                    Verificado = false,
                    Activo = true,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                };

                _context.Usuarios.Add(newUser);
                await _context.SaveChangesAsync();

                newUser = await _context.Usuarios
                        .Where(x => x.Correo == register.Correo &&
                                x.NombreUsuario == register.NombreUsuario &&
                                x.ContrasenaHash == register.ContrasenaHash)
                        .Select(x => new Usuario()
                        {
                            Id = x.Id,
                            NombreUsuario = x.NombreUsuario,
                            Correo = x.Correo,
                            Verificado = x.Verificado,
                            Activo = x.Activo,
                            Subscrito = x.Activo,
                            FechaFinSubscrito = x.FechaFinSubscrito,
                            FechaCreacion = x.FechaCreacion,
                            FechaActualizacion = x.FechaActualizacion
                        }).FirstOrDefaultAsync();

                return newUser;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<bool> CreateUserInfo( Usuario user )
        {
            try
            {
                UsuarioInfo newUserInfo = new UsuarioInfo()
                {
                    IdUsuario = user.Id,
                };

                _context.UsuarioInfos.Add(newUserInfo);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<Usuario> LoginUser( Login login )
        {
            try
            {
                Usuario User = await _context.Usuarios
                        .Where(x => x.Correo == login.email &&
                               x.ContrasenaHash == login.password &&
                               x.Activo == true)
                        .FirstOrDefaultAsync();

                if (User == null) { return null; }

                return User;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public string ObtenerHashKey()
        {
            try
            {
                string key = _configuration["Security:PasswordKey"];
                return key;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string ObtenerMailKey()
        {
            try
            {
                string key = _configuration["Security:MailKey"];
                return key;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Usuario> UsuarioXOtp(string otp)
        {
            try
            {
                string key = ObtenerHashKey();
                otp = Encript.EncriptarContra(otp, key);

                Usuario User = await _context.Usuarios
                        .Where(x => x.TokenRecuperacion == otp &&
                               x.Activo == true)
                        .FirstOrDefaultAsync();

                if (User == null) { return null; }

                return User;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
