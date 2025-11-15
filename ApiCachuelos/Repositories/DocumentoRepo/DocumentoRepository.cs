using System.Threading.Tasks;
using Entitys.CachuelosSA;
using Entitys.Entitys.Documentos;
using Microsoft.EntityFrameworkCore;
using Utils.Utilities;

namespace Repositories.DocumentoRepo
{
    public class DocumentoRepository : IDocumentoRepository
    {
        public readonly CachuelosSaContext _context;

        public DocumentoRepository(CachuelosSaContext context)
        {
            _context = context;
        }

        public async Task<DocResponse> CrearDocumento(Documentos documentos)
        {
            try
            {
                Documento newDocumento = new Documento()
                {
                    UrlDocumento = documentos.UrlString,
                    IdTipoDocumentos = documentos.idTipoDocumento,
                    Activo = true,
                    FechaIngreso = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                };

                _context.Add(newDocumento);
                await _context.SaveChangesAsync();

                DocResponse docRes = new DocResponse()
                {
                    Id = newDocumento.Id,
                    UrlDocumento = newDocumento.UrlDocumento,
                    IdTipoDocumentos = newDocumento.IdTipoDocumentos,
                    Activo = newDocumento.Activo
                };

                return docRes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UsuariosXDocumento> CrearUsuxDocumento(Usuario user, DocResponse documentos)
        {
            try
            {
                UsuariosXDocumento newUsuXDocu = new UsuariosXDocumento()
                {
                    IdUsuario = user.Id,
                    IdDocumento = documentos.Id,
                    FechaAsignacion = DateTime.Now,
                };

                _context.Add(newUsuXDocu);
                await _context.SaveChangesAsync();

                return newUsuXDocu;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<TipDoc>> ObtenerTiposDocs()
        {
            try
            {
                List<TipDoc> listaDocs = await _context.TipoDocumentos
                            .Select(x => new TipDoc
                            {
                                Id = x.Id,
                                TituloDoc = x.NombreDocumento,
                            }).ToListAsync();

                return listaDocs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Docus> ObtenerDocuCurriculumXIdUser(int idUser)
        {
            try
            {
                List<Docus> docu = await (
                        from UxD in _context.UsuariosXDocumentos
                        join Docu in _context.Documentos on UxD.IdDocumento equals Docu.Id
                        join TD in _context.TipoDocumentos on Docu.IdTipoDocumentos equals TD.Id
                        where   UxD.IdUsuario == idUser && 
                                TD.NombreDocumento == Const.TipoDocumento.Curriculum &&
                                Docu.Activo == true
                        select new Docus
                        {
                            Id = Docu.Id,
                            UrlDocumento = Docu.UrlDocumento
                        }
                    ).ToListAsync();

                return docu.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Docus> ObtenerDocuHistorialPoliXIdUser(int idUser)
        {
            try
            {
                List<Docus> docu = await (
                        from UxD in _context.UsuariosXDocumentos
                        join Docu in _context.Documentos on UxD.IdDocumento equals Docu.Id
                        join TD in _context.TipoDocumentos on Docu.IdTipoDocumentos equals TD.Id
                        where UxD.IdUsuario == idUser &&
                                TD.NombreDocumento == Const.TipoDocumento.HistoPoli &&
                                Docu.Activo == true
                        select new Docus
                        {
                            Id = Docu.Id,
                            UrlDocumento = Docu.UrlDocumento
                        }
                    ).ToListAsync();

                return docu.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Docus>> ObtenerDocuTitulosXIdUser(int idUser)
        {
            try
            {
                List<Docus> docu = await (
                        from UxD in _context.UsuariosXDocumentos
                        join Docu in _context.Documentos on UxD.IdDocumento equals Docu.Id
                        join TD in _context.TipoDocumentos on Docu.IdTipoDocumentos equals TD.Id
                        where UxD.IdUsuario == idUser &&
                                TD.NombreDocumento == Const.TipoDocumento.Titulo &&
                                Docu.Activo == true
                        select new Docus
                        {
                            Id = Docu.Id,
                            UrlDocumento = Docu.UrlDocumento
                        }
                    ).ToListAsync();

                return docu;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DesactivarDocumento(Docus documento)
        {
            try
            {
                Documento documentoEncontrado = await _context.Documentos
                            .FirstOrDefaultAsync(x => x.Id == documento.Id &&
                                    x.Activo == true);

                documentoEncontrado.Activo = false;
                _context.Update(documentoEncontrado);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<DocResponse> DesactivarDocumentoRespuesta(Docus documento)
        {
            try
            {
                Documento documentoEncontrado = await _context.Documentos
                            .FirstOrDefaultAsync(x => x.Id == documento.Id &&
                                    x.Activo == true);

                documentoEncontrado.Activo = false;
                _context.Update(documentoEncontrado);
                await _context.SaveChangesAsync();

                DocResponse respuesta = new DocResponse()
                {
                    Id = documentoEncontrado.Id,
                    UrlDocumento = documentoEncontrado.UrlDocumento,
                    IdTipoDocumentos = documentoEncontrado.IdTipoDocumentos,
                    Activo = documentoEncontrado.Activo
                };

                return respuesta;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> ObtenerIdDocuXNombre(string nombreDocu)
        {
            try
            {
                int idDocu = await _context.TipoDocumentos
                        .Where(x => x.NombreDocumento == nombreDocu)
                        .Select(x => x.Id).FirstOrDefaultAsync();

                return idDocu;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


    }
}
