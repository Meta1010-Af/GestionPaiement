using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GestionPaiement.Models
{
    public class EmailIdentity
    {
        // Adresse de l'expéditeur
        public string MailDeAdresse { get; set; }

        // Adresse du destinataire
        public string MailAAdresse { get; set; }

        // Objet du mail
        public string Sujet { get; set; }

        // Corps du message de l'email
        public string EmailcorpsMessage { get; set; }

        // Adresse de la copie (CC)
        public string CopieAAdresse { get; set; } // Optionnel, si on veut ajouter une copie

        // Validé à ne jamais être lié à la validation du modèle
        [ValidateNever]
        public string AttachementURL { get; set; } // URL du fichier à attacher (optionnel)

        // Données du fichier à attacher en mémoire
        public byte[] AttachementData { get; set; }  // Contenu du fichier (par exemple, PDF)

        // Nom du fichier à attacher
        public string AttachementNom { get; set; }   // Nom du fichier (par exemple, "bulletin-salaire.pdf")
    }
}
