using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Subasta;
using Entitys.Entitys.Trabajos;

namespace Entitys.Entitys.Subasta
{
    public class TrabajoImagenDto
    {
        public TrabajoDto Trabajo { get; set; }
        public List<string> ListImg { get; set; }
        public SubastaDto Subasta { get; set; }
        public List<SubastaOfertaDto> SubastaOfertas { get; set; }
    }

    public class SubastaDto
    {
        public int Id { get; set; }
        public int IdTrabajo { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }

    public class SubastaOfertaDto
    {
        public int Id { get; set; }
        public int IdSubasta { get; set; }
        public int IdUsuarioTrabajador { get; set; }
        public decimal Monto { get; set; }
        public string Mensaje { get; set; }
        public DateTime? FechaOferta { get; set; }
        public UsuarioSimpleDto Trabajador { get; set; }
    }

    public class OfertaRequest
    {
        public int idSubasta { get; set; }
        public decimal Monto { get; set; }
    }

    public class HistorialContratanteDto
    {
        public int IdTrabajo { get; set; }
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public DateTime? FechaTrabajo { get; set; }
        public string Estado { get; set; }
        public decimal? Monto { get; set; }
    }

    public class HistorialTrabajadorDto
    {
        public int IdTrabajo { get; set; }
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public DateTime? FechaTrabajo { get; set; }
        public decimal? PagoRecibido { get; set; }
        public string EstadoPago { get; set; }
    }

    public class SeleccionarGanadorRequest
    {
        public int IdSubasta { get; set; }
        public int IdOferta { get; set; }
    }

    public class SubirComprobanteRequest
    {
        public int idTrabajo { get; set; }
        public string url { get; set; }
    }

    public class ConfirmarFinalizacionRequest
    {
        public int idTrabajo { get; set; }
        public string otp { get; set; }
    }

    public class GanadorSeleccionadoDto
    {
        public string CorreoTrabajador { get; set; }
        public string NombreTrabajador { get; set; }
        public string TituloTrabajo { get; set; }

        public decimal MontoOferta { get; set; }
        public decimal Comision { get; set; }
        public decimal TotalPagar { get; set; }
    }

    public class GenerarOtpFinalRequest
    {
        public int IdTrabajo { get; set; }
    }

    public class ConfirmarInicioTrabajoRequest
    {
        public int IdTrabajo { get; set; }
        public string Otp { get; set; }
    }

    public class GenerarOtpInicioRequest
    {
        public int IdTrabajo { get; set; }
    }

    public class TrabajoActivoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public decimal? PrecioReferencial { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaTrabajo { get; set; }

        public CategoriaDto Categoria { get; set; }
        public UsuarioSimpleDto Creador { get; set; }
        public UsuarioSimpleDto TrabajadorAsignado { get; set; }

        public List<string> Imagenes { get; set; }

        public SubastaDto Subasta { get; set; }
        public List<PagoDto> Pagos { get; set; }
        public List<SubastaOfertaDto> Ofertas { get; set; }
    }
    public class UsuarioSimpleDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string UrlImg { get; set; }
    }

    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class PagoDto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
        public string EstadoPago { get; set; }
        public DateTime? FechaPago { get; set; }

        public List<ComprobantePagoDto> Comprobantes { get; set; }

        public PagoTrabajadorDto PagoTrabajador { get; set; }
    }

    public class ComprobantePagoDto
    {
        public int Id { get; set; }
        public string UrlComprobante { get; set; }
        public bool Validado { get; set; }
    }

    public class PagoTrabajadorDto
    {
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaTransferencia { get; set; }
    }

}
