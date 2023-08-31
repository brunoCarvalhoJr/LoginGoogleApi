using LoginGoogleApi.Models;
using System.Net.Mail;

namespace LoginGoogleApi.Services
{
    public class MailService
    {
        private static readonly string smtpClient = "localhost";
        private static readonly string satpEmail = "rest@gmail.com";
        private static readonly string satpName = "Testing";
        private static readonly int satpPort = 1025;

        public static async void SendPasswordResetMailAsync(ResetToken token)
        {
            SmtpClient client = new(smtpClient)
            {
                Port = satpPort,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                EnableSsl = false
            };

            MailMessage email = new()
            {
                From = new MailAddress(satpEmail, satpName),
                Subject = "Resete sua senha!",
                Body = $"Click <a href=\"http://localhost.3000/reset/{token.Token}\">aqui</a> para resetar sua senha!",
                IsBodyHtml = true
            };

            email.To.Add(new MailAddress(token.Email));

            await client.SendMailAsync(email);

            email.Dispose();
        }
    }
}
