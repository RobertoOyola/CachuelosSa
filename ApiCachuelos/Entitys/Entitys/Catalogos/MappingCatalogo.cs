using Entitys.CachuelosSA;
using Entitys.Entitys.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.Entitys.Catalogos
{
    public class MappingCatalogo
    {
        public static CatalogoDto MapearCatalogoDto(Catalogo cata)
        {
            CatalogoDto cataDto = new CatalogoDto();
            try
            {
                cataDto.Nombre = string.IsNullOrEmpty(cata.Nombre) ? string.Empty : cata.Nombre;
                cataDto.Codigo = string.IsNullOrEmpty(cata.Codigo) ? string.Empty : cata.Codigo;
                cataDto.Adicional = string.IsNullOrEmpty(cata.Adicional) ? string.Empty : cata.Adicional;

                return cataDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
