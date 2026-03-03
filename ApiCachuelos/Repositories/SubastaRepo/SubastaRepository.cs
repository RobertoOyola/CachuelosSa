using Entitys.CachuelosSA;
using Entitys.Entitys.Auth;
using Entitys.Entitys.Subasta;
using Entitys.Entitys.Trabajos;
using Microsoft.EntityFrameworkCore;
using Repositories.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Utilities;

namespace Repositories.SubastaRepo
{
    public class SubastaRepository: ISubastaRepository
    {
        private readonly CachuelosSaContext _context;

        public SubastaRepository(CachuelosSaContext context)
        {
            _context = context;
        }

        public async Task<Trabajo> CrearTrabajo(Trabajo trabajo)
        {
            try
            {
                _context.Trabajos.Add(trabajo);
                await _context.SaveChangesAsync();
                return trabajo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Subastum> CrearSubasta(Subastum subasta)
        {
            try
            {
                _context.Subasta.Add(subasta);
                await _context.SaveChangesAsync();
                return subasta;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TrabajoImagen> CrearTrabajoImagen(TrabajoImagen trabajoimg)
        {
            try
            {
                _context.TrabajoImagens.Add(trabajoimg);
                await _context.SaveChangesAsync();
                return trabajoimg;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Trabajo> TraerTrabajo(int idTrabajo)
        {
            try
            {
                Trabajo User = await _context.Trabajos
                        .Where(x => x.Id == idTrabajo &&
                               x.Activo == true)
                        .FirstOrDefaultAsync();

                if (User == null) { return null; }

                return User;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Trabajo>> TraerTrabajosPendientesList()
        {
            try
            {
                DateTime hoy = DateTime.Today;

                List<Trabajo> trabajos = await _context.Trabajos
                    .Where(x => x.Estado == Const.Estado_Tbj.Pendiente
                            && x.Activo == true
                            && x.FechaFinSubasta.HasValue
                            && x.FechaFinSubasta.Value.Date >= hoy)
                    .ToListAsync();

                if (trabajos == null) { return null; }

                return trabajos;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<string>> ImgxTrabajo(int idTrabajo)
        {
            try
            {
                var imgs = await _context.TrabajoImagens
                    .Where(x => x.IdTrabajo == idTrabajo && x.Activo == true)
                    .Select(x => x.UrlImagen)
                    .ToListAsync();

                return imgs;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        public async Task<Subastum> ObtenerSubasta(int idTrabajo)
        {
            try
            {
                Subastum Subasta = await _context.Subasta
                        .Where(x => x.IdTrabajo == idTrabajo)
                        .FirstOrDefaultAsync();

                if (Subasta == null) { return null; }

                return Subasta;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<SubastaOfertum>> ObtenerSubastaOfertas(int idSubasta)
        {
            try
            {
                List<SubastaOfertum> subastaOferta = await _context.SubastaOferta
                        .Where(x => x.IdSubasta == idSubasta)
                        .ToListAsync();

                return subastaOferta;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SubastaOfertum> CrearOferta(OfertaRequest ofertaRequest, int idUsuario)
        {
            SubastaOfertum subastaOfertum = new SubastaOfertum()
            {
                IdSubasta = ofertaRequest.idSubasta,
                IdUsuarioTrabajador = idUsuario,
                Monto = ofertaRequest.Monto,
                Mensaje = "",
                FechaOferta = DateTime.Now
            };

            await _context.SubastaOferta.AddAsync(subastaOfertum);
            await _context.SaveChangesAsync();

            return subastaOfertum;
        }

        public async Task<SubastaOfertum> TraerOfertas(OfertaRequest ofertaRequest, int idUsuario)
        {
            try
            {
                SubastaOfertum User = await _context.SubastaOferta
                        .Where(x => x.IdSubasta == ofertaRequest.idSubasta &&
                                    x.IdUsuarioTrabajador == idUsuario)
                        .FirstOrDefaultAsync();

                return User;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SubastaOfertum> ActualizarOfertas(SubastaOfertum ofertaRequest, decimal monto)
        {
            ofertaRequest.Monto = monto;
            _context.SubastaOferta.Update(ofertaRequest);
            await _context.SaveChangesAsync();

            return ofertaRequest;
        }

        public async Task<List<HistorialContratanteDto>> HistorialContratante(int idUsuario)
        {
            var historial = await _context.Trabajos
                .Where(t => t.IdUsuarioCreador == idUsuario && t.Estado == "FN")
                .Select(t => new HistorialContratanteDto
                {
                    IdTrabajo = t.Id,
                    Titulo = t.Titulo,
                    Categoria = t.IdCategoriaNavigation.Nombre,
                    FechaTrabajo = t.FechaTrabajo,
                    Estado = t.Estado,
                    Monto = _context.Pagos
                            .Where(p => p.IdTrabajo == t.Id)
                            .Select(p => p.Monto)
                            .FirstOrDefault()
                })
                .OrderByDescending(x => x.FechaTrabajo)
                .ToListAsync();

            return historial;
        }

        public async Task<List<HistorialTrabajadorDto>> HistorialTrabajador(int idUsuario)
        {
            var historial = await _context.SubastaOferta
                .Where(so => so.IdUsuarioTrabajador == idUsuario
                             && so.IdSubastaNavigation.IdTrabajoNavigation.Estado == "FN")
                .Select(so => new HistorialTrabajadorDto
                {
                    IdTrabajo = so.IdSubastaNavigation.IdTrabajoNavigation.Id,
                    Titulo = so.IdSubastaNavigation.IdTrabajoNavigation.Titulo,
                    Categoria = so.IdSubastaNavigation.IdTrabajoNavigation
                                    .IdCategoriaNavigation.Nombre,
                    FechaTrabajo = so.IdSubastaNavigation.IdTrabajoNavigation.FechaTrabajo,

                    PagoRecibido = (
                        from pt in _context.PagoTrabajadors
                        join p in _context.Pagos on pt.IdPago equals p.Id
                        where pt.IdUsuarioTrabajador == idUsuario
                              && p.IdTrabajo == so.IdSubastaNavigation.IdTrabajoNavigation.Id
                        select pt.Monto
                    ).FirstOrDefault(),

                    EstadoPago = (
                        from pt in _context.PagoTrabajadors
                        join p in _context.Pagos on pt.IdPago equals p.Id
                        where pt.IdUsuarioTrabajador == idUsuario
                              && p.IdTrabajo == so.IdSubastaNavigation.IdTrabajoNavigation.Id
                        select pt.Estado
                    ).FirstOrDefault()
                })
                .OrderByDescending(x => x.FechaTrabajo)
                .ToListAsync();

            return historial;
        }

        public async Task<GanadorSeleccionadoDto?> SeleccionarGanador(int idSubasta, int idOferta, int idCliente)
        {
            try
            {
                var subasta = await _context.Subasta
                    .FirstOrDefaultAsync(x => x.Id == idSubasta);

                if (subasta == null || subasta.Estado != Const.Estado_Oferta.Abierta)
                    return null;

                var trabajo = await _context.Trabajos
                    .FirstOrDefaultAsync(x => x.Id == subasta.IdTrabajo);

                if (trabajo == null
                    || trabajo.IdUsuarioCreador != idCliente
                    || trabajo.Estado != Const.Estado_Tbj.Pendiente)
                    return null;

                var oferta = await _context.SubastaOferta
                    .FirstOrDefaultAsync(x => x.Id == idOferta && x.IdSubasta == idSubasta);

                if (oferta == null)
                    return null;

                var trabajador = await _context.Usuarios
                    .FirstOrDefaultAsync(x => x.Id == oferta.IdUsuarioTrabajador);

                var trabajadorInfo = await _context.UsuarioInfos
                    .FirstOrDefaultAsync(x => x.IdUsuario == oferta.IdUsuarioTrabajador);

                if (trabajador == null || trabajadorInfo == null)
                    return null;

                trabajo.IdUsuarioTrabajadorAsignado = oferta.IdUsuarioTrabajador;
                trabajo.Estado = Const.Estado_Tbj.Asignado;

                subasta.Estado = Const.Estado_Oferta.Finalizada;

                decimal comision = oferta.Monto * 0.10m;
                decimal total = oferta.Monto + comision;

                Pago pago = new Pago()
                {
                    IdTrabajo = trabajo.Id,
                    IdUsuarioPagador = idCliente,
                    Monto = total,
                    MetodoPago = "Transferencia",
                    EstadoPago = Const.Estado_Pago.Pendiente
                };

                await _context.Pagos.AddAsync(pago);
                await _context.SaveChangesAsync();

                return new GanadorSeleccionadoDto
                {
                    CorreoTrabajador = trabajador.Correo,
                    NombreTrabajador = trabajadorInfo.Nombre,
                    TituloTrabajo = trabajo.Titulo,
                    MontoOferta = oferta.Monto,
                    Comision = comision,
                    TotalPagar = total
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> IniciarTrabajo(int idTrabajo)
        {
            var trabajo = await _context.Trabajos
                .FirstOrDefaultAsync(x => x.Id == idTrabajo);

            if (trabajo == null)
                return false;

            trabajo.Estado = Const.Estado_Tbj.En_Proceso;
            trabajo.FechaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SubirComprobante(int idTrabajo, string urlComprobante, int idCliente)
        {
            try
            {
                var pago = await _context.Pagos
                    .FirstOrDefaultAsync(x => x.IdTrabajo == idTrabajo);

                if (pago == null
                    || pago.IdUsuarioPagador != idCliente
                    || pago.EstadoPago != Const.Estado_Pago.Pendiente)
                    return false;

                ComprobantePago comp = new ComprobantePago()
                {
                    IdPago = pago.Id,
                    UrlComprobante = urlComprobante,
                    Validado = true
                };

                await _context.ComprobantePagos.AddAsync(comp);

                pago.EstadoPago = Const.Estado_Pago.Confirmado;

                var trabajo = await _context.Trabajos
                    .FirstOrDefaultAsync(x => x.Id == idTrabajo);

                if (trabajo == null || trabajo.Estado != Const.Estado_Tbj.Asignado)
                    return false;

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> FinalizarTrabajo(int idTrabajo)
        {
            try
            {
                var trabajo = await _context.Trabajos
                    .FirstOrDefaultAsync(x => x.Id == idTrabajo);

                if (trabajo == null
                    || trabajo.Estado != Const.Estado_Tbj.En_Proceso)
                    return false;

                var pago = await _context.Pagos
                    .FirstOrDefaultAsync(x => x.IdTrabajo == idTrabajo);

                if (pago == null
                    || pago.EstadoPago != Const.Estado_Pago.Confirmado)
                    return false;

                trabajo.Estado = Const.Estado_Tbj.Finalizado;

                PagoTrabajador pagoTrab = new PagoTrabajador()
                {
                    IdPago = pago.Id,
                    IdUsuarioTrabajador = trabajo.IdUsuarioTrabajadorAsignado.Value,
                    Monto = pago.Monto,
                    FechaTransferencia = DateTime.Now,
                    Estado = Const.Estado_Pago.Pagado_Trab
                };

                await _context.PagoTrabajadors.AddAsync(pagoTrab);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<TrabajoActivoDto>> ObtenerTrabajosActivos(int idUsuario)
        {
            try
            {
                return await _context.Trabajos
                    .Where(t =>
                        t.Activo == true &&
                        (
                            t.IdUsuarioCreador == idUsuario ||
                            t.IdUsuarioTrabajadorAsignado == idUsuario
                        ) &&
                        (
                            t.Estado == Const.Estado_Tbj.Pendiente ||
                            t.Estado == Const.Estado_Tbj.Asignado ||
                            t.Estado == Const.Estado_Tbj.En_Proceso
                        )
                    )
                    .Select(t => new TrabajoActivoDto
                    {
                        Id = t.Id,
                        Titulo = t.Titulo,
                        Descripcion = t.Descripcion,
                        Direccion = t.Direccion,
                        Latitud = t.Latitud,
                        Longitud = t.Longitud,
                        PrecioReferencial = t.PrecioReferencial,
                        Estado = t.Estado,
                        FechaTrabajo = t.FechaTrabajo,

                        Categoria = new CategoriaDto
                        {
                            Id = t.IdCategoriaNavigation.Id,
                            Nombre = t.IdCategoriaNavigation.Nombre
                        },

                        Creador = new UsuarioSimpleDto
                        {
                            Id = t.IdUsuarioCreadorNavigation.Id,
                            NombreUsuario = t.IdUsuarioCreadorNavigation.NombreUsuario,
                            UrlImg = t.IdUsuarioCreadorNavigation.UsuarioInfos
                                .Select(ui => ui.UrlImg)
                                .FirstOrDefault()
                        },

                        TrabajadorAsignado = t.IdUsuarioTrabajadorAsignadoNavigation == null
                            ? null
                            : new UsuarioSimpleDto
                            {
                                Id = t.IdUsuarioTrabajadorAsignadoNavigation.Id,
                                NombreUsuario = t.IdUsuarioTrabajadorAsignadoNavigation.NombreUsuario,
                                UrlImg = t.IdUsuarioTrabajadorAsignadoNavigation.UsuarioInfos
                                    .Select(ui => ui.UrlImg)
                                    .FirstOrDefault()
                            },


                        Imagenes = t.TrabajoImagens
                            .Where(i => i.Activo == true)
                            .Select(i => i.UrlImagen)
                            .ToList(),

                        Subasta = t.Subasta
                            .Select(s => new SubastaDto
                            {
                                Id = s.Id,
                                IdTrabajo = s.IdTrabajo,
                                Estado = s.Estado,
                                FechaInicio = s.FechaInicio,
                                FechaFin = s.FechaFin
                            })
                            .FirstOrDefault(),

                        Ofertas = t.Subasta
                            .SelectMany(s => s.SubastaOferta)
                            .Select(o => new SubastaOfertaDto
                            {
                                Id = o.Id,
                                IdSubasta = o.IdSubasta,
                                IdUsuarioTrabajador = o.IdUsuarioTrabajador,
                                Monto = o.Monto,
                                Mensaje = o.Mensaje,
                                FechaOferta = o.FechaOferta,
                                Trabajador = new UsuarioSimpleDto
                                {
                                    Id = o.IdUsuarioTrabajadorNavigation.Id,
                                    NombreUsuario = o.IdUsuarioTrabajadorNavigation.NombreUsuario,
                                    UrlImg = o.IdUsuarioTrabajadorNavigation.UsuarioInfos
                                        .Select(ui => ui.UrlImg)
                                        .FirstOrDefault()
                                }
                            })
                            .ToList(),

                        Pagos = t.Pagos
                            .Where(p => p.Activo == true)
                            .Select(p => new PagoDto
                            {
                                Id = p.Id,
                                Monto = p.Monto,
                                MetodoPago = p.MetodoPago,
                                EstadoPago = p.EstadoPago,
                                FechaPago = p.FechaPago,

                                Comprobantes = p.ComprobantePagos
                                    .Select(c => new ComprobantePagoDto
                                    {
                                        Id = c.Id,
                                        UrlComprobante = c.UrlComprobante,
                                        Validado = c.Validado.GetValueOrDefault()
                                    })
                                    .ToList(),

                                PagoTrabajador = p.PagoTrabajadors
                                    .Select(pt => new PagoTrabajadorDto
                                    {
                                        Monto = pt.Monto,
                                        Estado = pt.Estado,
                                        FechaTransferencia = pt.FechaTransferencia
                                    })
                                    .FirstOrDefault()
                            })
                            .ToList()
                    })
                    .ToListAsync();
            }
            catch
            {
                return new List<TrabajoActivoDto>();
            }
        }

        public async Task<(string correo, string nombre, string titulo, decimal monto)?>ObtenerDatosTransferencia(int idTrabajo)
        {
            try
            {
                var data = await (
                    from t in _context.Trabajos
                    join u in _context.Usuarios on t.IdUsuarioTrabajadorAsignado equals u.Id
                    join ui in _context.UsuarioInfos on u.Id equals ui.IdUsuario
                    join p in _context.Pagos on t.Id equals p.IdTrabajo
                    join pt in _context.PagoTrabajadors on p.Id equals pt.IdPago
                    where t.Id == idTrabajo
                    select new
                    {
                        Correo = u.Correo,
                        Nombre = ui.Nombre,
                        Titulo = t.Titulo,
                        Monto = pt.Monto
                    }
                ).FirstOrDefaultAsync();

                if (data == null)
                    return null;

                return (data.Correo, data.Nombre, data.Titulo, data.Monto);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Usuario> ObtenerUserXId(int idUser)
        {
            try
            {
                Usuario usuario = await _context.Usuarios
                            .Where(x => x.Id == idUser)
                            .FirstOrDefaultAsync();

                return usuario;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
