using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Mail;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Repositories.CatalogRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Utilities;

namespace Services.CatalogoSeri
{
    public class CatalogoService : ICatalogoService
    {
        private readonly ICatalogoRepository _cataRepo;
        private readonly IConfiguration _configuration;

        public CatalogoService(ICatalogoRepository cataRepo, IConfiguration configuration)
        {
            _cataRepo = cataRepo;
            _configuration = configuration;
        }

        public async Task<List<Catalogo>> GetCatalogoDetails(string nombreCatalogo)
        {
            List<Catalogo> info = new List<Catalogo>();

            info = await _cataRepo.ObtenerCatInfo(nombreCatalogo);

            return info;
        }

        public async Task<SmtpConfig> GetSmtpInfo()
        {
            Catalogo Host = await _cataRepo.ObtenerCodXCat(Const.Catalogos.SmtpConfig, Const.SmtpConfig.Host);
            Catalogo Port = await _cataRepo.ObtenerCodXCat(Const.Catalogos.SmtpConfig, Const.SmtpConfig.Port);
            Catalogo Mail = await _cataRepo.ObtenerCodXCat(Const.Catalogos.SmtpConfig, Const.SmtpConfig.Mail);
            string Password = _configuration["Security:MailKey"];

            SmtpConfig smtpConfig = new SmtpConfig
            {
                Host = Host?.Nombre ?? string.Empty,
                Port = Port.Nombre ?? string.Empty,
                Mail = Mail.Nombre ?? string.Empty,
                Password = Password ?? string.Empty
            };

            return smtpConfig;
        }

    }
}
