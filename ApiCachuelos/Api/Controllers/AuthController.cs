using Api.Entitys.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        [HttpPost(Name = "Login")]
        public string Login([FromBody] Login logInfo)
        {

            return "asd";
        }

    }
}
