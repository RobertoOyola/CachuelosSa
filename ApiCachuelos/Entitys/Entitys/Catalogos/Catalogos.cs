using Entitys.CachuelosSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.Entitys.Catalogos
{
    public class CatalogosUpdateInfo
    {
        public List<Catalogo> TipoIdentificacion { get; set; }
        public List<Catalogo> EstadoCivil { get; set; }
        public List<Catalogo> Nacionalidades { get; set; }
        public List<Catalogo> Provincias { get; set; }
        public List<Catalogo> Ciudades { get; set; }

    }
}
