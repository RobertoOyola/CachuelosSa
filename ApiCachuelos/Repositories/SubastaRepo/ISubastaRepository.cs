using Entitys.CachuelosSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SubastaRepo
{
    public interface ISubastaRepository
    {
        Task<Trabajo> CrearTrabajo(Trabajo trabajo);
        Task<TrabajoImagen> CrearTrabajoImagen(TrabajoImagen trabajoimg);
    }
}
