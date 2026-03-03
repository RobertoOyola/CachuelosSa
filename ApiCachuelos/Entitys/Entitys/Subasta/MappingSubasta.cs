using Entitys.CachuelosSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.Entitys.Subasta
{
    public class MappingSubasta
    {
        public static SubastaDto MapearSubastaDto(Subastum subasta)
        {
            if (subasta == null) return null;

            return new SubastaDto
            {
                Id = subasta.Id,
                IdTrabajo = subasta.IdTrabajo,
                Estado = string.IsNullOrEmpty(subasta.Estado) ? "" : subasta.Estado,
                FechaInicio = subasta.FechaInicio,
                FechaFin = subasta.FechaFin
            };
        }

        public static SubastaOfertaDto MapearSubastaOfertaDto(SubastaOfertum oferta)
        {
            if (oferta == null) return null;

            return new SubastaOfertaDto
            {
                Id = oferta.Id,
                IdSubasta = oferta.IdSubasta,
                IdUsuarioTrabajador = oferta.IdUsuarioTrabajador,
                Monto = oferta.Monto,
                Mensaje = string.IsNullOrEmpty(oferta.Mensaje) ? "" : oferta.Mensaje,
                FechaOferta = oferta.FechaOferta
            };
        }
    }
}
