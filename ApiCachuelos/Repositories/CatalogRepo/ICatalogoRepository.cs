using Entitys.CachuelosSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CatalogRepo
{
    public interface ICatalogoRepository
    {
        Task<List<Catalogo>> ObtenerCatInfo(string nombreCat);
        Task<Catalogo> ObtenerCodXCat(string nombreCat, string codigo);
    }
}
