using Entitys.CachuelosSA;
using Entitys.Entitys.Trabajos;
using Repositories.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Utilities;

namespace Repositories.SubastaRepo
{
    public class SubastaRepository: ISubastaRepository
    {
        private readonly CachuelosSaContext _context;

        public SubastaRepository(CachuelosSaContext context)
        {
            _context = context;
        }

        public async Task<Trabajo> CrearTrabajo(Trabajo trabajo)
        {
            try
            {
                _context.Trabajos.Add(trabajo);
                await _context.SaveChangesAsync();
                return trabajo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<TrabajoImagen> CrearTrabajoImagen(TrabajoImagen trabajoimg)
        {
            try
            {
                _context.TrabajoImagens.Add(trabajoimg);
                await _context.SaveChangesAsync();
                return trabajoimg;
            }
            catch (Exception ex)
            {
                return null;
            }
        }   

    }
}
