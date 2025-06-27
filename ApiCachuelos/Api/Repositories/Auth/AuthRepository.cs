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

        public async Task<Usuario> InsertUser( Register register )
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
    }
}
