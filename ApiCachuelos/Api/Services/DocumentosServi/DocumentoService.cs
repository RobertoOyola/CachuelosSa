using System.Threading.Tasks;
using Api.CachuelosSA;
using Api.Entitys;
using Api.Entitys.Documentos;
using Api.Entitys.Usuarios;
using Api.Repositories.DocumentoRepo;
using Api.Repositories.UsuarioRepo;
using Api.Services.Auth;
using Api.Utils;

namespace Api.Services.DocumentosServi
{
    public class DocumentoService : IDocumentoService
    {
        private readonly IUsuariosRepository _userRepo;
        private readonly IDocumentoRepository _docuRepo;
        private readonly IAuthService _authServ;

        public DocumentoService(IUsuariosRepository userRepo, 
                                IDocumentoRepository docuRepo, 
                                IAuthService authServ)
        {
            _userRepo = userRepo;
            _docuRepo = docuRepo;
            _authServ = authServ;
        }

        public async Task<ServiceResult<DocResponse>> InsertarDocumento(Documentos documento)
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            Usuario user = await _userRepo.ObtenerUserXId(usuario.Id);

            if (user == null) return ServiceResult<DocResponse>.Fail("Usuario no Encontrado o Bloqueado", 204);

            Docus busqudaDoc = new Docus();

            switch (documento.idTipoDocumento)
            {
                case Const.IdDocCurriculum:
                    busqudaDoc = await _docuRepo.ObtenerDocuCurriculumXIdUser(user.Id);
                    break;
                case Const.IdDocHistoPoli:
                    busqudaDoc = await _docuRepo.ObtenerDocuHistorialPoliXIdUser(user.Id);
                    break;
            }
            if (busqudaDoc != null)
                if (busqudaDoc.Id != 0 && busqudaDoc.UrlDocumento != null)
                {
                    bool Actualizo = await _docuRepo.DesactivarDocumento(busqudaDoc);
                }

            DocResponse newDocument = await _docuRepo.CrearDocumento(documento);

            if (newDocument == null) return ServiceResult<DocResponse>.Fail("No se pudo crear el Documento", 400);

            UsuariosXDocumento usuXDocu = await _docuRepo.CrearUsuxDocumento(user, newDocument);

            if (usuXDocu == null) return ServiceResult<DocResponse>.Fail("No se pudo crear el Documento", 400);

            return ServiceResult<DocResponse>.Ok(newDocument, "Documento Creado Con Exito", 201);
        }

        public async Task<ServiceResult<List<TipDoc>>> obtenerTiposDocs()
        {
            List<TipDoc> list = await _docuRepo.ObtenerTiposDocs();

            if (list == null || list.Count == 0)
                return ServiceResult<List<TipDoc>>.Fail("No se han encontrado tipo documentos", 204);

            return ServiceResult<List<TipDoc>>.Ok(list, "Lista Obtenida con Exito", 200);

        }

        public async Task<ServiceResult<ListDocumentos>> obtenerDocumentosXIdCliente()
        {
            Usuarios usuario = _authServ.OtenerTokenInfo();

            Docus docCorriculum = await _docuRepo.ObtenerDocuCurriculumXIdUser(usuario.Id);
            Docus docHistoPoli = await _docuRepo.ObtenerDocuHistorialPoliXIdUser(usuario.Id);
            List<Docus> titulos = await _docuRepo.ObtenerDocuTitulosXIdUser(usuario.Id);

            ListDocumentos documentos = new ListDocumentos()
            {
                Curriculum = docCorriculum,
                Titulos = titulos,
                HistorialPolicial = docHistoPoli
            };

            if (docCorriculum == null && docHistoPoli == null && (titulos == null || titulos.Count == 0))
                return ServiceResult<ListDocumentos>.Fail("El Usuario no tiene Documentos", 204);

            return ServiceResult<ListDocumentos>.Ok(documentos, "Documentos Obtenidos con Exito", 200);
        }

        public async Task<ServiceResult<DocResponse>> EliminarDocumento(Docus documento)
        {
            DocResponse docResponse = await _docuRepo.DesactivarDocumentoRespuesta(documento);

            if (docResponse == null) return ServiceResult<DocResponse>.Fail("No se pudo eliminar el Documento", 409);

            return ServiceResult<DocResponse>.Ok(docResponse, "Documento Eliminado Con Exito", 200);

        }
    }
}
