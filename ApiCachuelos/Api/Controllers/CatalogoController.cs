using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Catalogos;
using Entitys.Entitys.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Auth;
using Services.CatalogoSeri;
using Utils.Utilities;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoController : ControllerBase
    {
        private readonly ICatalogoService _cataServ;

        public CatalogoController(ICatalogoService cataServ)
        {
            _cataServ = cataServ;
        }

        [Authorize]
        [HttpPost("InfoParaRegister")]
        public async Task<IActionResult> InfoParaRegister()
        {
            ServiceResult<CatalogosUpdateInfo> result = await _cataServ.InfoParaRegister();

            if (!result.Exitoso)
            {
                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<CatalogosUpdateInfo>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("ObtenerCatalogoTrabajo")]
        public async Task<IActionResult> ObtenerCatalogoTrabajo()
        {
            ServiceResult<List<CategoriaTrabajo>> result = await _cataServ.ObtenerCataTrabajo();

            if (!result.Exitoso)
            {
                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<List<CategoriaTrabajo>>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("ObtenerIdCatalogoTrabajo")]
        public async Task<IActionResult> ObtenerIdCatalogoTrabajo([FromBody] string nomCatT)
        {
            ServiceResult<int> result = await _cataServ.ObtenerIdCatTrabajo(nomCatT);

            if (!result.Exitoso)
            {
                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<int>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }
    }
}
