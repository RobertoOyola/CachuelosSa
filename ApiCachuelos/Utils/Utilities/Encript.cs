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
    }
}
