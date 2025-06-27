using System.Threading.Tasks;
using Api.CachuelosSA;
using Api.Entitys;
using Api.Entitys.Auth;
using Api.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authServ;

        public AuthController(IAuthService authServ)
        {
            _authServ = authServ;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login logInfo)
        {
            var result = await _authServ.Login(logInfo);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<Usuario>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Register registerInfo)
        {
            var result = await _authServ.Register(registerInfo);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<Usuario>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

    }
}
