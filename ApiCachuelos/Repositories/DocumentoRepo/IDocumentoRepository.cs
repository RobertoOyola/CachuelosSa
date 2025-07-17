using Entitys.CachuelosSA;
using Entitys.Entitys.Documentos;

namespace Repositories.DocumentoRepo
{
    public interface IDocumentoRepository
    {
        Task<DocResponse> CrearDocumento(Documentos documentos);
        Task<UsuariosXDocumento> CrearUsuxDocumento(Usuario user, DocResponse documentos);
        Task<List<TipDoc>> ObtenerTiposDocs();
        Task<Docus> ObtenerDocuCurriculumXIdUser(int idUser);
        Task<Docus> ObtenerDocuHistorialPoliXIdUser(int idUser);
        Task<List<Docus>> ObtenerDocuTitulosXIdUser(int idUser);
        Task<bool> DesactivarDocumento(Docus documento);
        Task<DocResponse> DesactivarDocumentoRespuesta(Docus documento);
        Task<int> ObtenerIdDocuXNombre(string nombreDocu);
    }
}
