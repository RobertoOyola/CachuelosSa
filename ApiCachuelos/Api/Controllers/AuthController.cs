using System.Threading.Tasks;
using Api.CachuelosSA;
using Api.Entitys;
using Api.Entitys.Auth;
using Api.Entitys.Usuarios;
using Api.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
            ServiceResult<Usuario> result = await _authServ.Login(logInfo);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            string token = _authServ.GenerarToken(result.Datos);

            Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok(new CustomResponse<object>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = new { token, result.Datos }
            });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Register registerInfo)
        {
            ServiceResult<Usuario> result = await _authServ.Register(registerInfo);

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

        [Authorize]
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok(new CustomResponse<object>
            {
                Header = new CustomHeader { Codigo = 200, Mensaje = "Session Cerrada con Exito" }
            });
        }

        [Authorize]
        [HttpGet("check")]
        public IActionResult Check()
        {
            return Ok(new { isAuthenticated = true });
        }


    }
}
