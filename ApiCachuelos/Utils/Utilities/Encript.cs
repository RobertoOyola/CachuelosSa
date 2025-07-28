using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Utilities
{
    public class Encript
    {
        public static string EncriptarContra(string contra, string key)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(contra)));
        }

        public static string CrearOtp()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[4]; // 32 bits
            rng.GetBytes(bytes);
            int value = BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF; // Elimina el bit de signo
            int otp = value % 1000000; // Asegura que sea de 6 dígitos

            return otp.ToString("D6");
        }
    }
}
