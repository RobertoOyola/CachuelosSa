using Azure;
using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Trabajos;
using Entitys.Entitys.Usuarios;
using Repositories.CatalogRepo;
using Repositories.SubastaRepo;
using Repositories.UsuarioRepo;
using Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.SubastaServi
{
    public class SubastaService : ISubastaService
    {
        private readonly ISubastaRepository _subaRepo;
        private readonly ICatalogoRepository _cataRepo;
        private readonly IAuthService _authServ;
        public SubastaService(ISubastaRepository subaRepo, IAuthService authServ)
        {
            _subaRepo = subaRepo;
            _authServ = authServ;
        }

        public async Task<ServiceResult<TrabajoDto>> CrearTrabajo(TrabajoRequest trabajoRequest)
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            if (comprobarInfoTrabajo(trabajoRequest)) 
                return ServiceResult<TrabajoDto>.Fail("Falta informacion", 204);

            TrabajoDto trabajoDto = new TrabajoDto();
            trabajoDto = MappingTrabajo.MapearTrabajoDto(trabajoRequest);

            if (trabajoDto == null)
                return ServiceResult<TrabajoDto>.Fail("Error en la conversion de datos", 400);

            trabajoDto.IdUsuarioCreador = usuario.Id;

            Trabajo trabajo = new Trabajo();
            trabajo = MappingTrabajo.MapearTrabajo(trabajoDto);

            if (trabajo == null)
                return ServiceResult<TrabajoDto>.Fail("Error en la conversion de datos", 400);

            if (usuario.EsSuscriptor == "true") trabajo.Especial = true;

            trabajo = await _subaRepo.CrearTrabajo(trabajo);
            if (trabajo == null)
                return ServiceResult<TrabajoDto>.Fail("Error en el guardado de datos", 400);

            foreach (string trabajoimg in trabajoRequest.ImagenesUrls)
            {
                TrabajoImagen trabajoImagen = new TrabajoImagen()
                {
                    IdTrabajo = trabajo.Id,
                    UrlImagen = trabajoimg,
                    FechaIngreso = DateTime.Now,
                    Activo = true
                };

                await _subaRepo.CrearTrabajoImagen(trabajoImagen);
            }

            trabajoDto = MappingTrabajo.MapearTrabajoDto(trabajo);

            if (trabajoDto == null)
                return ServiceResult<TrabajoDto>.Fail("Error en la conversion de datos", 400);

            return ServiceResult<TrabajoDto>.Ok(trabajoDto, "Informacion Guardada con Exito", 200);

        }

        public bool comprobarInfoTrabajo(TrabajoRequest trabajoRequest)
        {
            if (trabajoRequest == null) return true;
            if (trabajoRequest.Titulo == null || trabajoRequest.Titulo == "") return true;
            if (trabajoRequest.Descripcion == null || trabajoRequest.Descripcion == "") return true;
            if (trabajoRequest.Direccion == null || trabajoRequest.Direccion == "") return true;
            if (trabajoRequest.Latitud == null) return true;
            if (trabajoRequest.Longitud == null) return true;
            if (trabajoRequest.PrecioReferencial == null) trabajoRequest.PrecioReferencial = 0;

            return false;
        }
    }
}
