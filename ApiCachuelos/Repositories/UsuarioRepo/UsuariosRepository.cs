using System.Threading.Tasks;
using Entitys.CachuelosSA;
using Entitys.Entitys.Auth;
using Microsoft.EntityFrameworkCore;

namespace Repositories.UsuarioRepo
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly CachuelosSaContext _context;

        public UsuariosRepository(CachuelosSaContext context)
        {
            _context = context;
        }

        public async Task<Usuario> ObtenerUserXId(int idUser)
        {
            try
            {
                Usuario usuario = await _context.Usuarios
                            .Where(x => x.Id == idUser && x.Activo == true)
                            .Select(x => new Usuario()
                            {
                                Id = x.Id,
                                NombreUsuario = x.NombreUsuario,
                                Correo = x.Correo,
                                Verificado = x.Verificado,
                                Activo = x.Activo,
                                RolId = x.RolId,
                                FechaCreacion = x.FechaCreacion,
                                FechaUltimoLogin = x.FechaUltimoLogin,
                                FechaActualizacion = x.FechaActualizacion,
                                TokenRecuperacion = x.TokenRecuperacion,
                                ExpiracionToken = x.ExpiracionToken
                            }).FirstOrDefaultAsync();

                return usuario;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UsuarioInfo> ObtenerUserInfoXId(int idUser)
        {
            try
            {
                UsuarioInfo usuarioinfo = await _context.UsuarioInfos
                            .Where(x => x.IdUsuario == idUser && x.Activo == true)
                            .Select(x => new UsuarioInfo()
                            {
                                Id = x.Id,
                                IdUsuario = x.IdUsuario,
                                UrlImg = x.UrlImg,
                                Descripcion = x.Descripcion,
                                Activo = x.Activo,
                                FechaUltimaConexion = x.FechaUltimaConexion,
                                FechaActualizacion = x.FechaActualizacion,
                            }).FirstOrDefaultAsync();

                return usuarioinfo;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UsuarioInfo> ActualizarUserInfoXId(UsuarioInfo userInfo)
        {
            try
            {
                userInfo.FechaActualizacion = DateTime.Now;
                _context.Update(userInfo);
                await _context.SaveChangesAsync();

                return userInfo;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Usuario> ActualizarUsuario(Usuario user)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();

                user.ContrasenaHash = string.Empty;

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
