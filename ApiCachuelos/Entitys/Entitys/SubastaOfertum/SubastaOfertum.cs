using Entitys.CachuelosSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys.Entitys.SubastaOfertum
{
    public partial class SubastaOfertum
    {
        public int Id { get; set; }
        public int IdSubasta { get; set; }
        public int IdUsuarioTrabajador { get; set; }
        public decimal Monto { get; set; }
        public string Mensaje { get; set; }
        public DateTime? FechaOferta { get; set; }
    }
}
