using System.Security.Claims;
using System.Threading.Tasks;
using Entitys.Entitys;
using Entitys.Entitys.Documentos;
using Entitys.Entitys.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Auth;
using Services.DocumentosServi;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoController : ControllerBase
    {
        private readonly IDocumentoService _docServ;
        private readonly IAuthService _authServ;

        public DocumentoController(IDocumentoService docServ, IAuthService authService)
        {
            _docServ = docServ;
            _authServ = authService;
        }

        [Authorize]
        [HttpPost("CrearDocumento")]
        public async Task<IActionResult> CrearDocumento(Documentos documentos)
        {

            Usuarios usuarioToken = _authServ.OtenerTokenInfo();

            if (usuarioToken == null)
                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = 401, Mensaje = "Token Modificado o Sin Token" },
                    Body = null
                });

            ServiceResult<DocResponse> result = await _docServ.InsertarDocumento(documentos);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<DocResponse>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("ObtenerTipoDocumentos")]
        public async Task<IActionResult> ObtenerTipoDocumentos()
        {
            ServiceResult<List<TipDoc>> result = await _docServ.obtenerTiposDocs();

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<List<TipDoc>>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("ObtenerDocumentosUsuario")]
        public async Task<IActionResult> ObtenerDocumentosDeUsuario()
        {
            ServiceResult<ListDocumentos> result = await _docServ.obtenerDocumentosXIdCliente();

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<ListDocumentos>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }

        [Authorize]
        [HttpPost("EliminarDocumento")]
        public async Task<IActionResult> EliminarDocumento(Docus documentos)
        {

            ServiceResult<DocResponse> result = await _docServ.EliminarDocumento(documentos);

            if (!result.Exitoso)
            {

                return BadRequest(new CustomResponse<string>
                {
                    Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                    Body = null
                });
            }

            return Ok(new CustomResponse<DocResponse>
            {
                Header = new CustomHeader { Codigo = result.Codigo, Mensaje = result.Mensaje },
                Body = result.Datos
            });
        }
    }
}
