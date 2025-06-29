using Api.Entitys.Documentos;
using Api.Entitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Services.UsersServi;
using Api.Entitys.Usuarios;
using Api.CachuelosSA;

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
    }
}
