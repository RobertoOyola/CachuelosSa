using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Utilities
{
    public class Const
    {
        public class Token
        {
            public const string Id = "Id";
            public const string Usuario = "Usuario";
            public const string Rol = "RolId";
            public const string EsSuscriptor = "EsSubcriptor";
        }

        public class TipoDocumento
        {
            public const string Curriculum = "Curriculum";
            public const string Titulo = "Titulos";
            public const string HistoPoli = "Historial_Policial";
            public const string Cedula = "Cedula";
            public const string ComprobanteP = "Comprobante de pago";
        }

        public class Catalogos
        {
            public const string TIdentificacion = "TIPO_IDENTIFICACION";
            public const string EstadoCivil = "ESTADO_CIVIL";
            public const string SmtpConfig = "SMTP_CONFIG";
            public const string Nacionalidades = "NACIONALIDADES";
            public const string Provincias = "PROVINCIAS";
            public const string Ciudades = "CIUDADES";
            public const string TipoOtp = "TIPO_OTP";
        }
        
        public class SmtpConfig
        {
            public const string Host = "HOST";
            public const string Port = "PORT";
            public const string Mail = "MAIL";
        }

        public class OtpTipo
        {
            public const string VerificarUsu = "VU";
            public const string CambioContra = "CC";
            public const string EliminarUsu = "EU";
            public const string IniciarTbj = "IT";
            public const string FinalizarTbj = "FT";
            public const string DesbloquearUsu = "DU";
        }

        public class Estado_Tbj
        {
            public const string Pendiente = "PE";
            public const string Asignado = "AS";
            public const string En_Proceso = "EP";
            public const string Finalizado = "FN";
            public const string Cancelado = "CN";
        }

        public class Estado_Oferta
        {
            public const string Abierta = "AB";
            public const string Finalizada = "FI";
            public const string Cancelada = "CA";
        }

        public class Estado_Pago
        {
            public const string Pendiente = "PE";
            public const string Confirmado = "CO";
            public const string Rechazado = "RE";
            public const string Pendiente_Trab = "PE";
            public const string Pagado_Trab = "PA";
        }
    }
}
