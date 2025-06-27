using Api.Entitys.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;

namespace Api.Utils
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
