using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Subasta;
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
        Task<ServiceResult<List<TrabajoImagenDto>>> CargarSubastas();
        Task<ServiceResult<SubastaOfertum>> CrearOferta(OfertaRequest ofertaRequest);
        Task<ServiceResult<List<HistorialContratanteDto>>> HistorialContratante();
        Task<ServiceResult<List<HistorialTrabajadorDto>>> HistorialTrabajador();
        Task<ServiceResult<string>> SeleccionarGanador(SeleccionarGanadorRequest seleccionarGanadorRequest);
        Task<ServiceResult<string>> SubirComprobante(SubirComprobanteRequest subirComprobanteRequest);
        Task<ServiceResult<string>> GenerarOtpInicio(int idTrabajo);
        Task<ServiceResult<string>> GenerarOtpFinal(int idTrabajo);
        Task<ServiceResult<string>> ConfirmarFinalizacion(ConfirmarFinalizacionRequest confirmarFinalizacion);
        Task<ServiceResult<List<TrabajoActivoDto>>> TrabajosActivos();
        Task<ServiceResult<string>> ConfirmarInicioTrabajo(int idTrabajo, string otp);
    }
}
