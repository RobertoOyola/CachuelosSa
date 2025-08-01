using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.UsersServi;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsuarioService _userServ;
        public UserController(IUsuarioService userServ)
        {
            _userServ = userServ;
        }

        [Authorize]
        [HttpPost("CambiarFotoUsuario")]
        public async Task<IActionResult> CambiarFotoUsuario([FromBody] string fotoId)
        {

            ServiceResult<UsuarioInfo> result = await _userServ.CambiarFotoUsuario(fotoId);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<UsuarioInfo>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("ObtenerUser")]
        public async Task<IActionResult> ObtenerUsuario()
        {

            ServiceResult<UsuarioxUsuarioInfo> result = await _userServ.ObtenerUsuario();

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<UsuarioxUsuarioInfo>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("ObtenerUsuarioOtros")]
        public async Task<IActionResult> ObtenerUsuario([FromBody] int id)
        {

            ServiceResult<UsuarioxUsuarioInfo> result = await _userServ.ObtenerUsuario(id);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<UsuarioxUsuarioInfo>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("ActualizarUsuario")]
        public async Task<IActionResult> ActualizarUsuario([FromBody] UsuarioInfoDto request)
        {

            ServiceResult<UsuarioInfo> result = await _userServ.ActualizarUsuario(request);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<UsuarioInfo>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }
    }
}
