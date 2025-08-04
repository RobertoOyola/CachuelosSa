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
        public List<CatalogoDto> TipoIdentificacion { get; set; }
        public List<CatalogoDto> EstadoCivil { get; set; }
        public List<CatalogoDto> Nacionalidades { get; set; }
        public List<CatalogoDto> Provincias { get; set; }
        public List<CatalogoDto> Ciudades { get; set; }

    }

    public class CatalogoDto
    {
        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string Adicional { get; set; }
    }
}
