using Entitys.CachuelosSA;
using Entitys.Entitys.Usuarios;

namespace Entitys.Entitys.Trabajos
{
    public class MappingTrabajo
    {
        public static Trabajo MapearTrabajo(TrabajoDto dto)
        {
            try
            {
                Trabajo trabajo = new Trabajo();

                trabajo.IdUsuarioCreador = dto.IdUsuarioCreador != 0 ? dto.IdUsuarioCreador : trabajo.IdUsuarioCreador;
                trabajo.IdCategoria = dto.IdCategoria != 0 ? dto.IdCategoria : trabajo.IdCategoria;
                trabajo.Titulo = string.IsNullOrEmpty(dto.Titulo) ? trabajo.Titulo : dto.Titulo;
                trabajo.Descripcion = string.IsNullOrEmpty(dto.Descripcion) ? trabajo.Descripcion : dto.Descripcion;
                trabajo.Direccion = string.IsNullOrEmpty(dto.Direccion) ? trabajo.Direccion : dto.Direccion;
                trabajo.Latitud = dto.Latitud ?? trabajo.Latitud;
                trabajo.Longitud = dto.Longitud ?? trabajo.Longitud;
                trabajo.PrecioReferencial = dto.PrecioReferencial ?? trabajo.PrecioReferencial;
                trabajo.Especial = dto.Especial ?? trabajo.Especial;
                trabajo.Estado = string.IsNullOrEmpty(dto.Estado) ? trabajo.Estado : dto.Estado;
                trabajo.FechaPublicacion = dto.FechaPublicacion ?? trabajo.FechaPublicacion;
                trabajo.FechaActualizacion = dto.FechaActualizacion ?? trabajo.FechaActualizacion;
                trabajo.Activo = dto.Activo ?? trabajo.Activo;

                return trabajo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static TrabajoDto MapearTrabajoDto(TrabajoRequest request)
        {
            try
            {
                TrabajoDto dto = new TrabajoDto();

                dto.Id = 0;
                dto.IdUsuarioCreador = 0;

                dto.IdCategoria = request.IdCategoria != 0 ? request.IdCategoria : 0;
                dto.Titulo = string.IsNullOrEmpty(request.Titulo) ? "" : request.Titulo;
                dto.Descripcion = string.IsNullOrEmpty(request.Descripcion) ? "" : request.Descripcion;
                dto.Direccion = string.IsNullOrEmpty(request.Direccion) ? "" : request.Direccion;
                dto.Latitud = request.Latitud ?? null;
                dto.Longitud = request.Longitud ?? null;
                dto.PrecioReferencial = request.PrecioReferencial ?? null;
                dto.Especial = false;
                dto.Estado = "PE";
                dto.FechaPublicacion = DateTime.Now;
                dto.FechaActualizacion = DateTime.Now;
                dto.Activo = true;

                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static TrabajoDto MapearTrabajoDto(Trabajo trabajo)
        {
            try
            {
                TrabajoDto dto = new TrabajoDto();

                dto.Id = trabajo.Id;
                dto.IdUsuarioCreador = trabajo.IdUsuarioCreador;
                dto.IdCategoria = trabajo.IdCategoria;

                dto.Titulo = string.IsNullOrEmpty(trabajo.Titulo) ? "" : trabajo.Titulo;
                dto.Descripcion = string.IsNullOrEmpty(trabajo.Descripcion) ? "" : trabajo.Descripcion;
                dto.Direccion = string.IsNullOrEmpty(trabajo.Direccion) ? "" : trabajo.Direccion;

                dto.Latitud = trabajo.Latitud ?? null;
                dto.Longitud = trabajo.Longitud ?? null;
                dto.PrecioReferencial = trabajo.PrecioReferencial ?? null;

                dto.Especial = trabajo.Especial ?? false;

                dto.Estado = string.IsNullOrEmpty(trabajo.Estado) ? "" : trabajo.Estado;

                dto.FechaPublicacion = trabajo.FechaPublicacion ?? null;
                dto.FechaActualizacion = trabajo.FechaActualizacion ?? null;

                dto.Activo = trabajo.Activo ?? false;

                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
