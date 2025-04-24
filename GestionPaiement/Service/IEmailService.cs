using GestionPaiement.Models;

namespace GestionPaiement.Service
{
    public interface IEmailService
    {
        Task EnvoyerEmailAsync(EmailIdentity email);
    }
}
