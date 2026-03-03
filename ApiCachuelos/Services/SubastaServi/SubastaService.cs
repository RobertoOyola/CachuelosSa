using Azure;
using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Mail;
using Entitys.Entitys.Subasta;
using Entitys.Entitys.Trabajos;
using Entitys.Entitys.Usuarios;
using Microsoft.EntityFrameworkCore;
using Repositories.CatalogRepo;
using Repositories.SubastaRepo;
using Services.Auth;
using Services.CatalogoSeri;
using System.Threading.Tasks;
using Utils.Utilities;

namespace Services.SubastaServi
{
    public class SubastaService : ISubastaService
    {
        private readonly ISubastaRepository _subaRepo;
        private readonly ICatalogoService _cataServ;
        private readonly IAuthService _authServ;
        public SubastaService(ISubastaRepository subaRepo, IAuthService authServ, ICatalogoService cataServ)
        {
            _subaRepo = subaRepo;
            _authServ = authServ;
            _cataServ = cataServ;
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

            Subastum subasta = new Subastum()
            {
                IdTrabajo = trabajo.Id,
                Estado = Const.Estado_Oferta.Abierta,
                FechaInicio = DateTime.Now,
                FechaFin = trabajo.FechaFinSubasta
            };

            subasta = await _subaRepo.CrearSubasta(subasta);

            if (trabajoDto == null)
                return ServiceResult<TrabajoDto>.Fail("Error al crear la subasta", 400);

            return ServiceResult<TrabajoDto>.Ok(trabajoDto, "Informacion Guardada con Exito", 200);

        }

        public async Task<ServiceResult<List<TrabajoImagenDto>>> CargarSubastas()
        {
            List<TrabajoImagenDto> result = new List<TrabajoImagenDto>();
            List<Trabajo> trabajos = await _subaRepo.TraerTrabajosPendientesList();

            foreach (Trabajo trabajo in trabajos)
            {
                List<string> imagenes = new List<string>();
                imagenes = await _subaRepo.ImgxTrabajo(trabajo.Id);
                Subastum subastum = await _subaRepo.ObtenerSubasta(trabajo.Id);
                List<SubastaOfertum> subastaOfertas = await _subaRepo.ObtenerSubastaOfertas(subastum.Id);

                TrabajoDto trabajoDto = MappingTrabajo.MapearTrabajoDto(trabajo);
                SubastaDto subastaDto = MappingSubasta.MapearSubastaDto(subastum);
                List<SubastaOfertaDto> subastaOfertaDto = new List<SubastaOfertaDto>();
                foreach (SubastaOfertum subastaOfertum in subastaOfertas)
                {
                    subastaOfertaDto.Add(MappingSubasta.MapearSubastaOfertaDto(subastaOfertum));
                }

                TrabajoImagenDto trabajoImagenDto = new TrabajoImagenDto()
                {
                    Trabajo = trabajoDto,
                    ListImg = imagenes,
                    Subasta = subastaDto,
                    SubastaOfertas = subastaOfertaDto
                };

                result.Add(trabajoImagenDto);
            }

            if (result == null)
                return ServiceResult<List<TrabajoImagenDto>>.Fail("Error al traer la informacion", 400);

            return ServiceResult<List<TrabajoImagenDto>>.Ok(result, "Informacion Traida con Exito", 200);
        }

        public async Task<ServiceResult<SubastaOfertum>> CrearOferta(OfertaRequest ofertaRequest)
        {
            SubastaOfertum subastaOfertum = new SubastaOfertum();
            Usuarios usuario = _authServ.OtenerTokenInfo();

            subastaOfertum = await _subaRepo.TraerOfertas(ofertaRequest, usuario.Id);
            if (subastaOfertum == null)
                subastaOfertum = await _subaRepo.CrearOferta(ofertaRequest, usuario.Id);
            else
                subastaOfertum = await _subaRepo.ActualizarOfertas(subastaOfertum, ofertaRequest.Monto);
            
            if (subastaOfertum == null)
                return ServiceResult<SubastaOfertum>.Fail("Error al procesar la oferta", 400);

            return ServiceResult<SubastaOfertum>.Ok(subastaOfertum, "Oferta procesada con Exito", 200);
        }

        public async Task<ServiceResult<List<HistorialContratanteDto>>> HistorialContratante()
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            if (usuario == null)
                return ServiceResult<List<HistorialContratanteDto>>
                    .Fail("No autorizado", 401);

            var historial = await _subaRepo.HistorialContratante(usuario.Id);
            historial ??= new List<HistorialContratanteDto>();

            return ServiceResult<List<HistorialContratanteDto>>
                .Ok(historial, "OK", 200);
        }

        public async Task<ServiceResult<List<HistorialTrabajadorDto>>> HistorialTrabajador()
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            if (usuario == null)
                return ServiceResult<List<HistorialTrabajadorDto>>
                    .Fail("No autorizado", 401);

            var historial = await _subaRepo.HistorialTrabajador(usuario.Id);
            historial ??= new List<HistorialTrabajadorDto>();

            return ServiceResult<List<HistorialTrabajadorDto>>
                .Ok(historial, "OK", 200);
        }

        public async Task<ServiceResult<string>> SeleccionarGanador(SeleccionarGanadorRequest request)
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            if (usuario == null)
                return ServiceResult<string>.Fail("Usuario no autorizado", 401);

            var result = await _subaRepo.SeleccionarGanador(
                request.IdSubasta,
                request.IdOferta,
                usuario.Id
            );

            if (result == null)
                return ServiceResult<string>.Fail("Error al asignar trabajador", 400);

            SmtpConfig smtpConfig = await _cataServ.GetSmtpInfo();

            string htmlBody = Mail.GanadorSeleccionadoMail(
                result.NombreTrabajador,
                result.TituloTrabajo,
                result.MontoOferta
            );

            await Mail.SendEmailAsync(
                result.CorreoTrabajador,
                "Has sido seleccionado para un trabajo",
                htmlBody,
                smtpConfig
            );

            return ServiceResult<string>.Ok(
                "Trabajador asignado correctamente",
                "OK",
                200
            );
        }

        public async Task<ServiceResult<string>> SubirComprobante(SubirComprobanteRequest request)
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            if (usuario == null)
                return ServiceResult<string>.Fail("Usuario no autorizado", 401);

            bool result = await _subaRepo.SubirComprobante(request.idTrabajo, request.url, usuario.Id);

            if (!result)
                return ServiceResult<string>.Fail("Error al subir comprobante", 400);

            return ServiceResult<string>.Ok("Comprobante validado correctamente", "OK", 200);
        }

        public async Task<ServiceResult<string>> GenerarOtpInicio(int idTrabajo)
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            if (usuario == null)
                return ServiceResult<string>.Fail("Usuario no autorizado", 401);

            var trabajo = await _subaRepo.TraerTrabajo(idTrabajo);

            if (trabajo == null)
                return ServiceResult<string>.Fail("Trabajo no encontrado", 404);

            if (trabajo.IdUsuarioCreador != usuario.Id)
                return ServiceResult<string>.Fail("No autorizado", 403);

            Usuario trabajador = await _subaRepo.ObtenerUserXId(trabajo.IdUsuarioTrabajadorAsignado ?? 0);

            if (trabajador == null)
                return ServiceResult<string>.Fail("Trabajador no asignado", 400);

            await _authServ.EnviarCorreoOtp(
                new MailInfo { Mail = trabajador.Correo },
                Const.OtpTipo.IniciarTbj
            );

            return ServiceResult<string>.Ok("OTP enviado al trabajador", "OK", 200);
        }

        public async Task<ServiceResult<string>> GenerarOtpFinal(int idTrabajo)
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            if (usuario == null)
                return ServiceResult<string>.Fail("No autorizado", 401);

            var trabajo = await _subaRepo.TraerTrabajo(idTrabajo);

            if (trabajo == null)
                return ServiceResult<string>.Fail("Trabajo no encontrado", 404);

            if (trabajo.IdUsuarioTrabajadorAsignado != usuario.Id)
                return ServiceResult<string>.Fail("No autorizado", 403);

            if (trabajo.Estado != Const.Estado_Tbj.En_Proceso)
                return ServiceResult<string>.Fail("El trabajo no está en proceso", 400);

            var cliente = await _subaRepo.ObtenerUserXId(trabajo.IdUsuarioCreador);

            await _authServ.EnviarCorreoOtp(
                new MailInfo { Mail = cliente.Correo },
                Const.OtpTipo.FinalizarTbj
            );

            return ServiceResult<string>.Ok(
                "OTP final enviado al cliente",
                "OK",
                200
            );
        }

        public async Task<ServiceResult<string>> ConfirmarInicioTrabajo(int idTrabajo, string otp)
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            if (usuario == null)
                return ServiceResult<string>.Fail("No autorizado", 401);

            var trabajo = await _subaRepo.TraerTrabajo(idTrabajo);

            if (trabajo == null)
                return ServiceResult<string>.Fail("Trabajo no encontrado", 404);

            if (trabajo.IdUsuarioTrabajadorAsignado != usuario.Id)
                return ServiceResult<string>.Fail("No autorizado", 403);

            var validacion = await _authServ.VerificarOtp(otp);

            if (!validacion.Exitoso)
                return ServiceResult<string>.Fail(validacion.Mensaje, 400);

            bool result = await _subaRepo.IniciarTrabajo(idTrabajo);

            if (!result)
                return ServiceResult<string>.Fail("Error al iniciar trabajo", 400);

            return ServiceResult<string>.Ok("Trabajo iniciado correctamente", "OK", 200);
        }

        public async Task<ServiceResult<string>> ConfirmarFinalizacion(ConfirmarFinalizacionRequest request)
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            if (usuario == null)
                return ServiceResult<string>.Fail("No autorizado", 401);

            var trabajo = await _subaRepo.TraerTrabajo(request.idTrabajo);

            if (trabajo == null || trabajo.IdUsuarioCreador != usuario.Id)
                return ServiceResult<string>.Fail("No autorizado", 403);

            var validacion = await _authServ.VerificarOtp(request.otp);

            if (!validacion.Exitoso)
                return ServiceResult<string>.Fail(validacion.Mensaje, 400);

            bool result = await _subaRepo.FinalizarTrabajo(request.idTrabajo);

            if (!result)
                return ServiceResult<string>.Fail("Error al finalizar trabajo", 400);

            try
            {
                var datos = await _subaRepo.ObtenerDatosTransferencia(request.idTrabajo);

                if (datos != null)
                {
                    SmtpConfig smtpConfig = await _cataServ.GetSmtpInfo();

                    string htmlBody = Mail.TransferenciaConfirmadaMail(
                        datos.Value.nombre,
                        datos.Value.titulo,
                        datos.Value.monto
                    );

                    await Mail.SendEmailAsync(
                        datos.Value.correo,
                        "Transferencia confirmada - Cachuelos S.A.",
                        htmlBody,
                        smtpConfig
                    );
                }
            }
            catch { }
            return ServiceResult<string>.Ok("Trabajo finalizado y pago liberado", "OK", 200);
        }

        public async Task<ServiceResult<List<TrabajoActivoDto>>> TrabajosActivos()
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            if (usuario == null)
                return ServiceResult<List<TrabajoActivoDto>>
                    .Fail("No autorizado", 401);

            var trabajos = await _subaRepo
                .ObtenerTrabajosActivos(usuario.Id);

            trabajos ??= new List<TrabajoActivoDto>();

            return ServiceResult<List<TrabajoActivoDto>>
                .Ok(trabajos, "OK", 200);
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
