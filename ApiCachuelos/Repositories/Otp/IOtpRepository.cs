using Entitys.CachuelosSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Otp
{
    public interface IOtpRepository
    {
        Task<OtpAction> CrearOtp (int idUsuario, string otp, string tipoOtp, int tiempoExpira);
        Task<OtpAction> ObtenerOtpXOtp(string otp);
        Task<OtpAction> ObtenerOtpXOtpTipo(string otp, string TipoOtp);
        Task<OtpAction> UsarOtp (OtpAction otpAction);
        Task<bool> EliminarOtpsNoUsadas(int idUsuario, string tipoOtp);
    }
}
