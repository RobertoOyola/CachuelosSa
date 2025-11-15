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
    }
}
