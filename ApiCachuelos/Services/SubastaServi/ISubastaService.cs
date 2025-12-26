using Entitys.Entitys;
using Entitys.Entitys.Trabajos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SubastaServi
{
    public interface ISubastaService
    {
        Task<ServiceResult<TrabajoDto>> CrearTrabajo(TrabajoRequest trabajoRequest);
    }
}
