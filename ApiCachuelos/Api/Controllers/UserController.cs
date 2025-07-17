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
        public async Task<IActionResult> CambiarFotoUsuario(UsuariosInfoDto usuariosInfo)
        {

            ServiceResult<UsuarioInfo> result = await _userServ.CambiarFotoUsuario(usuariosInfo);

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
        [HttpPost("CambiarDescripcionUsuario")]
        public async Task<IActionResult> CambiarDescripcionUsuario(UsuariosInfoDto usuariosInfo)
        {

            ServiceResult<UsuarioInfo> result = await _userServ.CambiarDescripcionUsuario(usuariosInfo);

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
        [HttpPost("ObtenerUserInfo")]
        public async Task<IActionResult> ObtenerUserInfo()
        {

            ServiceResult<UsuarioXInfoCompleta> result = await _userServ.ObtenerInfoUsuario();

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<UsuarioXInfoCompleta>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }
    }
}
