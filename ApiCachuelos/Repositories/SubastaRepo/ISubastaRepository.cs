using Entitys.CachuelosSA;
using Entitys.Entitys.Subasta;
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
        Task<Trabajo> TraerTrabajo(int idTrabajo);
        Task<Subastum> CrearSubasta(Subastum subasta);
        Task<List<Trabajo>> TraerTrabajosPendientesList();
        Task<List<string>> ImgxTrabajo(int idTrabajo);
        Task<List<SubastaOfertum>> ObtenerSubastaOfertas(int idSubasta);
        Task<Subastum> ObtenerSubasta(int idTrabajo);
        Task<SubastaOfertum> CrearOferta(OfertaRequest ofertaRequest, int idUsuario);
        Task<SubastaOfertum> TraerOfertas(OfertaRequest ofertaRequest, int idUsuario);
        Task<SubastaOfertum> ActualizarOfertas(SubastaOfertum ofertaRequest, decimal monto);
        Task<List<HistorialContratanteDto>> HistorialContratante(int idUsuario);
        Task<List<HistorialTrabajadorDto>> HistorialTrabajador(int idUsuario);
        Task<GanadorSeleccionadoDto?> SeleccionarGanador(int idSubasta, int idOferta, int idCliente);
        Task<bool> SubirComprobante(int idTrabajo, string urlComprobante, int idCliente);
        Task<bool> FinalizarTrabajo(int idTrabajo);
        Task<Usuario> ObtenerUserXId(int idUser);
        Task<List<TrabajoActivoDto>> ObtenerTrabajosActivos(int idUsuario);
        Task<bool> IniciarTrabajo(int idTrabajo);
        Task<(string correo, string nombre, string titulo, decimal monto)?> ObtenerDatosTransferencia(int idTrabajo);

    }
}
