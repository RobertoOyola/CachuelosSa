using Entitys.CachuelosSA;
using Microsoft.EntityFrameworkCore;
using Repositories.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Utilities;

namespace Repositories.Otp
{
    public class OtpRepository : IOtpRepository
    {
        private readonly CachuelosSaContext _context;
        private readonly IAuthRepository _authRepo;

        public OtpRepository(CachuelosSaContext context, IAuthRepository authRepo)
        {
            _context = context;
            _authRepo = authRepo;
        }

        public async Task<OtpAction> CrearOtp(int idUsuario, string otp , string tipoOtp, int tiempoExpira)
        {
            try
            {
                string key = _authRepo.ObtenerHashKey();

                OtpAction otpAction = new OtpAction
                {
                    IdUsuario = idUsuario,
                    CodigoOtp = Encript.EncriptarContra(otp, key),
                    TipoOtp = tipoOtp,
                    Expiracion = DateTime.Now.AddMinutes(tiempoExpira)
                };

                _context.OtpActions.Add(otpAction);
                await _context.SaveChangesAsync();
                return otpAction;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<OtpAction> ObtenerOtpXOtp(string otp)
        {
            try
            {
                string key = _authRepo.ObtenerHashKey();
                otp = Encript.EncriptarContra(otp, key);

                OtpAction otpAction = await _context.OtpActions
                    .Where(x => x.CodigoOtp == otp &&
                                x.Activo == true &&
                                x.Usado == false)
                    .FirstOrDefaultAsync();

                if (otpAction == null)
                    return null;
               
                return otpAction;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<OtpAction> ObtenerOtpXOtpTipo(string otp, string TipoOtp)
        {
            try
            {
                string key = _authRepo.ObtenerHashKey();
                otp = Encript.EncriptarContra(otp, key);

                OtpAction otpAction = await _context.OtpActions
                    .Where(x => x.CodigoOtp == otp &&
                                x.Activo == true &&
                                x.Usado == false &&
                                x.TipoOtp == TipoOtp)
                    .FirstOrDefaultAsync();

                if (otpAction == null)
                    return null;

                return otpAction;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<OtpAction> UsarOtp (OtpAction otpAction)
        {
            try
            {
                otpAction.Usado = true;
                _context.OtpActions.Update(otpAction);
                await _context.SaveChangesAsync();
                return otpAction;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> EliminarOtpsNoUsadas(int idUsuario, string tipoOtp)
        {
            try
            {
                List<OtpAction> otpActions = await _context.OtpActions
                                    .Where(x => x.IdUsuario == idUsuario &&
                                                x.Usado == false &&
                                                x.Activo == true)
                                    .ToListAsync();

                if (otpActions == null || otpActions.Count() == 0)
                    return true;

                foreach(OtpAction otp in otpActions)
                {
                    otp.Activo = false;
                    _context.Update(otp);
                }

                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
