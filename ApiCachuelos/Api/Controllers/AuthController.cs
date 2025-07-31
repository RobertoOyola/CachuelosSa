using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Auth;
using Entitys.Entitys.Mail;
using Entitys.Entitys.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Auth;
using System.Threading.Tasks;
using Utils.Utilities;

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

            string token = await _authServ.GenerarToken(result.Datos);

            Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(4)
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

        [HttpPost("EmailOtpVerificarUsu")]
        public async Task<IActionResult> EmailOtpVerificarUsu([FromBody] MailInfo mailInfo)
        {
            ServiceResult<MailReturn> result = await _authServ.EnviarCorreoOtp(mailInfo, Const.OtpTipo.VerificarUsu);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<MailReturn>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [HttpPost("EmailOtpCambioContra")]
        public async Task<IActionResult> EmailOtpCambioContra([FromBody] MailInfo mailInfo)
        {
            ServiceResult<MailReturn> result = await _authServ.EnviarCorreoOtp(mailInfo, Const.OtpTipo.CambioContra);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<MailReturn>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [HttpPost("EmailOtpEliminarUsu")]
        public async Task<IActionResult> EmailOtpEliminarUsu([FromBody] MailInfo mailInfo)
        {
            ServiceResult<MailReturn> result = await _authServ.EnviarCorreoOtp(mailInfo, Const.OtpTipo.EliminarUsu);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<MailReturn>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [HttpPost("EmailOtpIniciarTbj")]
        public async Task<IActionResult> EmailOtpIniciarTbj([FromBody] MailInfo mailInfo)
        {
            ServiceResult<MailReturn> result = await _authServ.EnviarCorreoOtp(mailInfo, Const.OtpTipo.IniciarTbj);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<MailReturn>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [HttpPost("EmailOtpFinalizarTbj")]
        public async Task<IActionResult> EmailOtpFinalizarTbj([FromBody] MailInfo mailInfo)
        {
            ServiceResult<MailReturn> result = await _authServ.EnviarCorreoOtp(mailInfo, Const.OtpTipo.FinalizarTbj);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<MailReturn>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [HttpPost("VerificarOtp")]
        public async Task<IActionResult> VerificarOtp([FromBody] IngresoOtp Otp)
        {
            ServiceResult<Usuario> result = await _authServ.VerificarOtp(Otp.Otp);

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

        [HttpPost("RecuperarContrasena")]
        public async Task<IActionResult> RecuperarContrasena([FromBody] RecuperarContrasena recuperar)
        {
            ServiceResult<string> result = await _authServ.CambiarContrasena(recuperar);

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

    }
}
