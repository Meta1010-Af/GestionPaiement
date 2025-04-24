using GestionPaiement.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GestionPaiement.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnvoyerEmailAsync(EmailIdentity email)
        {
            var smtpSettings = _configuration.GetSection("SMTPSettings");
            var smtpClient = new SmtpClient(smtpSettings["Host"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                EnableSsl = bool.Parse(smtpSettings["UseSSL"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["Username"]),
                Subject = email.Sujet,
                Body = email.EmailcorpsMessage,
                IsBodyHtml = false
            };

            mailMessage.To.Add(email.MailAAdresse);

            // Ajout de la copie (CC) si elle est fournie
            if (!string.IsNullOrEmpty(email.CopieAAdresse))
            {
                mailMessage.CC.Add(email.CopieAAdresse);
            }

            // Ajouter une pièce jointe si présente
            if (!string.IsNullOrEmpty(email.AttachementNom) && email.AttachementData != null)
            {
                var attachment = new Attachment(new System.IO.MemoryStream(email.AttachementData), email.AttachementNom);
                mailMessage.Attachments.Add(attachment);
            }

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log ou gestion des erreurs si nécessaire
                throw new InvalidOperationException("Erreur lors de l'envoi de l'email", ex);
            }
        }
    }
}
