using Entitys.Entitys.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using static System.Net.WebRequestMethods;

namespace Utils.Utilities
{
    public class Mail
    {
        public static async Task<MailReturn> SendEmailAsync(string toEmail, string subject, string messageBody, SmtpConfig config)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(config.Mail));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = messageBody
                };

                int port = int.TryParse(config.Port, out int result) ? result : 0;

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(config.Host, port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(config.Mail, config.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return new MailReturn
                {
                    Ok = true,
                    Message = "Correo enviado con Exito"
                };
            }
            catch (Exception ex)
            {
                return new MailReturn
                {
                    Ok = false,
                    Message = "Error al enviar el correo: " + ex.Message
                };
            }
        }

        public static string OtpMail(string otp)
        {
            string htmlBody = $@"
            <html>
                <body style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;'>
                    <div style='max-width: 500px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 30px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                        <h2 style='color: #333333;'>Verificación de Seguridad</h2>
                        <p style='font-size: 16px; color: #555555;'>Tu código de verificación es:</p>
                        <div style='font-size: 32px; font-weight: bold; color: #903eb0; text-align: center; margin: 20px 0;'>{otp}</div>
                        <p style='font-size: 14px; color: #888888;'>Este código expira en 10 minutos. No compartas este código con nadie.</p>
                        <hr />
                        <p style='font-size: 12px; color: #aaaaaa;'>Cachuelos S.A - Todos los derechos reservados</p>
                    </div>
                </body>
            </html>";

            return htmlBody;
        }

        public static string GanadorSeleccionadoMail(string nombre, string tituloTrabajo, decimal monto)
        {
            string htmlBody = $@"
            <html>
                <body style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;'>
                    <div style='max-width: 500px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 30px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                        <h2 style='color: #333333;'>¡Fuiste seleccionado!</h2>
                        <p style='font-size: 16px; color: #555555;'>
                            Hola <b>{nombre}</b>,
                        </p>
                        <p style='font-size: 16px; color: #555555;'>
                            Has sido seleccionado para realizar el siguiente trabajo:
                        </p>
                        <div style='background:#f3f0ff; padding:15px; border-radius:8px; margin:15px 0;'>
                            <h3 style='margin:0; color:#6c5ce7;'>{tituloTrabajo}</h3>
                            <p style='margin:5px 0; font-size:14px;'>Monto acordado: <b>${monto}</b></p>
                        </div>
                        <p style='font-size: 14px; color: #888888;'>
                            Ingresa a la plataforma para ver los detalles y coordinar el inicio del trabajo.
                        </p>
                        <hr />
                        <p style='font-size: 12px; color: #aaaaaa;'>Cachuelos S.A - Todos los derechos reservados</p>
                    </div>
                </body>
            </html>";

            return htmlBody;
        }

        public static string TransferenciaConfirmadaMail(string nombre, string tituloTrabajo, decimal monto)
        {
            string htmlBody = $@"
            <html>
                <body style='font-family: Arial, sans-serif; background-color: #f4f6f8; padding: 20px;'>
                    <div style='max-width: 500px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 30px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
                
                        <h2 style='color: #2d3436;'>Transferencia Confirmada</h2>
                
                        <p style='font-size: 16px; color: #555555;'>
                            Hola <b>{nombre}</b>,
                        </p>

                        <p style='font-size: 16px; color: #555555;'>
                            El cliente ha confirmado la finalización del siguiente trabajo:
                        </p>

                        <div style='background:#eafaf1; padding:15px; border-radius:8px; margin:15px 0;'>
                            <h3 style='margin:0; color:#27ae60;'>{tituloTrabajo}</h3>
                            <p style='margin:5px 0; font-size:14px;'>
                                Monto transferido: <b>${monto}</b>
                            </p>
                        </div>

                        <p style='font-size: 14px; color: #555555;'>
                            El pago ha sido liberado correctamente.
                        </p>

                        <hr />

                        <p style='font-size: 12px; color: #aaaaaa;'>
                            Cachuelos S.A - Confirmación automática de pago
                        </p>

                    </div>
                </body>
            </html>";
            return htmlBody;
        }
    }
}
