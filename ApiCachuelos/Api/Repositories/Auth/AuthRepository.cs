using Api.CachuelosSA;
using Api.Entitys.Auth;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly CachuelosSaContext _context;

        public AuthRepository(CachuelosSaContext context)
        {
            _context = context;
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

                newUser.ContrasenaHash = "";

                return newUser;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<Usuario> LoginUser(Login login)
        {
            try
            {
                Usuario User = await _context.Usuarios
                        .Where(x => x.Correo == login.email &&
                                            x.ContrasenaHash == login.password)
                        .Select(x => new Usuario()
                        {
                            Id = x.Id,
                            NombreUsuario = x.NombreUsuario,
                            Correo = x.Correo,
                            Verificado = x.Verificado,
                            Activo = x.Activo,
                            FechaCreacion = x.FechaCreacion,
                            FechaUltimoLogin = x.FechaUltimoLogin,
                            FechaActualizacion = x.FechaActualizacion,
                            Rol = x.Rol,
                            TokenRecuperacion = x.TokenRecuperacion,
                            ExpiracionToken = x.ExpiracionToken

                        }).FirstAsync();

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
