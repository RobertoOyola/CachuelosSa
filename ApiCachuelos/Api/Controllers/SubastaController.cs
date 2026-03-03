using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Auth;
using Entitys.Entitys.Subasta;
using Entitys.Entitys.Trabajos;
using Entitys.Entitys.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.SubastaServi;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubastaController : ControllerBase
    {
        private readonly ISubastaService _subaServ;

        public SubastaController(ISubastaService subaServ)
        {
            _subaServ = subaServ;
        }

        [Authorize]
        [HttpPost("CrearTrabajo")]
        public async Task<IActionResult> CrearTrabajo([FromBody] TrabajoRequest trabajoRequest)
        {
            ServiceResult<TrabajoDto> result = await _subaServ.CrearTrabajo(trabajoRequest);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<TrabajoDto>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("TraerTrabajos")]
        public async Task<IActionResult> TraerTrabajos()
        {
            ServiceResult<List<TrabajoImagenDto>> result = await _subaServ.CargarSubastas();

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<List<TrabajoImagenDto>>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("CrearOferta")]
        public async Task<IActionResult> CrearOferta([FromBody] OfertaRequest ofertaRequest)
        {
            ServiceResult<SubastaOfertum> result = await _subaServ.CrearOferta(ofertaRequest);

            if (!result.Exitoso)
            {
                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<SubastaOfertum>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("HistorialContratante")]
        public async Task<IActionResult> HistorialContratante()
        {
            ServiceResult<List<HistorialContratanteDto>> result = await _subaServ.HistorialContratante();

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<List<HistorialContratanteDto>>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("HistorialTrabajador")]
        public async Task<IActionResult> HistorialTrabajador()
        {
            ServiceResult<List<HistorialTrabajadorDto>> result = await _subaServ.HistorialTrabajador();

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<List<HistorialTrabajadorDto>>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("SeleccionarGanador")]
        public async Task<IActionResult> SeleccionarGanador([FromBody] SeleccionarGanadorRequest seleccionarGanadorRequest)
        {
            ServiceResult<string> result = await _subaServ.SeleccionarGanador(seleccionarGanadorRequest);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<string>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("SubirComprobante")]
        public async Task<IActionResult> SubirComprobante([FromBody] SubirComprobanteRequest subirComprobanteRequest)
        {
            ServiceResult<string> result = await _subaServ.SubirComprobante(subirComprobanteRequest);

            if (!result.Exitoso)
            {
                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<string>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("GenerarOtpInicio")]
        public async Task<IActionResult> GenerarOtpInicio([FromBody] GenerarOtpInicioRequest request)
        {
            ServiceResult<string> result = await _subaServ.GenerarOtpInicio(request.IdTrabajo);

            if (!result.Exitoso)
            {
                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<string>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("GenerarOtpFinal")]
        public async Task<IActionResult> GenerarOtpFinal([FromBody] GenerarOtpFinalRequest request)
        {
            ServiceResult<string> result = await _subaServ.GenerarOtpFinal(request.IdTrabajo);

            if (!result.Exitoso)
            {
                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<string>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("ConfirmarFinalizacion")]
        public async Task<IActionResult> ConfirmarFinalizacion([FromBody] ConfirmarFinalizacionRequest confirmarFinalizacion)
        {
            ServiceResult<string> result = await _subaServ.ConfirmarFinalizacion(confirmarFinalizacion);

            if (!result.Exitoso)
            {
                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<string>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("ConfirmarInicioTrabajo")]
        public async Task<IActionResult> ConfirmarInicioTrabajo([FromBody] ConfirmarInicioTrabajoRequest request)
        {
            ServiceResult<string> result = await _subaServ.ConfirmarInicioTrabajo(request.IdTrabajo, request.Otp);

            if (!result.Exitoso)
            {
                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<string>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("TrabajosActivos")]
        public async Task<IActionResult> TrabajosActivos()
        {
            ServiceResult<List<TrabajoActivoDto>> result = await _subaServ.TrabajosActivos();

            if (!result.Exitoso)
            {
                return BadRequest(new CustomResponse<List<TrabajoActivoDto>>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<List<TrabajoActivoDto>>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }
    }
}
