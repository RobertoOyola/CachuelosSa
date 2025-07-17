using Entitys.Entitys;
using Entitys.Entitys.Documentos;

namespace Services.DocumentosServi
{
    public interface IDocumentoService
    {
        Task<ServiceResult<DocResponse>> InsertarDocumento(Documentos documento);
        Task<ServiceResult<List<TipDoc>>> obtenerTiposDocs();
        Task<ServiceResult<ListDocumentos>> obtenerDocumentosXIdCliente();
        Task<ServiceResult<DocResponse>> EliminarDocumento(Docus documento);
    }
}
