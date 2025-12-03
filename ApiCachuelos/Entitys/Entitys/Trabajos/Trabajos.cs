using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.Entitys.Trabajos
{
    public class TrabajoDto
    {
        public int Id { get; set; }
        public int IdUsuarioCreador { get; set; }
        public int IdCategoria { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public decimal? PrecioReferencial { get; set; }
        public bool? Especial { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool? Activo { get; set; }
    }

    public class TrabajoRequest
    {
        public int IdCategoria { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public decimal? PrecioReferencial { get; set; }
    }
}
