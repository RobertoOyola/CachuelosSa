using Entitys.CachuelosSA;
using Entitys.Entitys.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CatalogoSeri
{
    public interface ICatalogoService
    {
        Task<List<Catalogo>> GetCatalogoDetails(string nombreCatalogo);
        Task<SmtpConfig> GetSmtpInfo();
    }
}
