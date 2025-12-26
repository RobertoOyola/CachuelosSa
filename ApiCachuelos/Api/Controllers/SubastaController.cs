using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Auth;
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
    }
}
