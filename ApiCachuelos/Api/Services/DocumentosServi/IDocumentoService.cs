using Api.CachuelosSA;
using Api.Entitys.Documentos;
using Api.Entitys;

namespace Api.Services.DocumentosServi
{
    public interface IDocumentoService
    {
        Task<ServiceResult<DocResponse>> InsertarDocumento(Documentos documento);
        Task<ServiceResult<List<TipDoc>>> obtenerTiposDocs();
        Task<ServiceResult<ListDocumentos>> obtenerDocumentosXIdCliente();
        Task<ServiceResult<DocResponse>> EliminarDocumento(Docus documento);
    }
}
