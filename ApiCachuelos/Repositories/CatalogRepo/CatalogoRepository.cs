using Entitys.CachuelosSA;
using Entitys.Entitys.Documentos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CatalogRepo
{
    public class CatalogoRepository : ICatalogoRepository
    {
        public readonly CachuelosSaContext _context;

        public CatalogoRepository(CachuelosSaContext context)
        {
            _context = context;
        }

        public async Task<List<Catalogo>> ObtenerCatInfo(string nombreCat)
        {
            try
            {
                List<Catalogo> listaDocs = await _context.Catalogos
                            .Where(x => x.Activo == true 
                                    && x.NombreCat == nombreCat)
                            .ToListAsync();

                return listaDocs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Catalogo> ObtenerCodXCat(string nombreCat, string codigo)
        {
            try
            {
                Catalogo listaDocs = await _context.Catalogos
                            .Where(x => x.Activo == true
                                    && x.NombreCat == nombreCat
                                    && x.Codigo == codigo)
                            .FirstOrDefaultAsync();

                return listaDocs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<CategoriaTrabajo>> ObtenerCatTrabajo()
        {
            try
            {
                List<CategoriaTrabajo> listaTrabj = await _context.CategoriaTrabajos
                            .Where(x => x.Activo == true)
                            .ToListAsync();

                return listaTrabj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CategoriaTrabajo> ObtenerCatTrabajo(string nombreT)
        {
            try
            {
                CategoriaTrabajo listaTrabj = await _context.CategoriaTrabajos
                            .FirstOrDefaultAsync(x => x.Activo == true && x.Nombre == nombreT);

                return listaTrabj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CategoriaTrabajo> CrearCatTrabajo(string nombreT)
        {
            CategoriaTrabajo catT = new CategoriaTrabajo()
            {
                Nombre = nombreT,
                Descripcion = "",
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            try
            {
                _context.Add(catT);
                await _context.SaveChangesAsync();

                CategoriaTrabajo listaTrabj = await _context.CategoriaTrabajos
                            .FirstOrDefaultAsync(x => x.Nombre == nombreT);

                return listaTrabj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
