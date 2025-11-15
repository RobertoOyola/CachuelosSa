using Entitys.CachuelosSA;
using Entitys.Entitys;
using Entitys.Entitys.Catalogos;
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

        public async Task<ServiceResult<CatalogosUpdateInfo>> InfoParaRegister()
        {
            List<Catalogo> TipoIdentificacion = await _cataRepo.ObtenerCatInfo(Const.Catalogos.TIdentificacion);
            List<Catalogo> EstadoCivil = await _cataRepo.ObtenerCatInfo(Const.Catalogos.EstadoCivil);
            List<Catalogo> Nacionalidades = await _cataRepo.ObtenerCatInfo(Const.Catalogos.Nacionalidades);
            List<Catalogo> Provincias = await _cataRepo.ObtenerCatInfo(Const.Catalogos.Provincias);
            List<Catalogo> Ciudades = await _cataRepo.ObtenerCatInfo(Const.Catalogos.Ciudades);

            if (TipoIdentificacion == null || EstadoCivil == null || Nacionalidades == null || Provincias == null || Ciudades == null)
                return ServiceResult<CatalogosUpdateInfo>.Fail("Alguno de los Catalogos estan vacios", 404);

            CatalogosUpdateInfo result = new CatalogosUpdateInfo()
            {
                TipoIdentificacion = ListCataToListCataDto(TipoIdentificacion),
                EstadoCivil = ListCataToListCataDto(EstadoCivil),
                Nacionalidades = ListCataToListCataDto(Nacionalidades),
                Provincias = ListCataToListCataDto(Provincias),
                Ciudades = ListCataToListCataDto(Ciudades)
            };

            return ServiceResult<CatalogosUpdateInfo>.Ok(result, "Catalogos obtenidos con Exito", 200);

        }

        private List<CatalogoDto> ListCataToListCataDto(List<Catalogo> listCata)
        {
            List<CatalogoDto> listCataDto = new List<CatalogoDto>();

            foreach (Catalogo cata in listCata)
            {
                CatalogoDto cataDto = MappingCatalogo.MapearCatalogoDto(cata);
                listCataDto.Add(cataDto);
            }

            return listCataDto;
        }

    }
}
