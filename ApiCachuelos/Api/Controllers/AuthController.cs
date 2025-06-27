using System.Threading.Tasks;
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
        public IActionResult Login([FromBody] Login logInfo)
        {
            return Ok("Login exitoso");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Register registerInfo)
        {
            await _authServ.Register(registerInfo);
            return Ok("Registro exitoso");
        }

    }
}
