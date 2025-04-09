using System.ComponentModel.DataAnnotations;

namespace GestionPaiement.Models.DataModel
{
    public class Paiement
    {
        [Key]
        public int IdPaiement { get; set; }
        public int AgentId { get; set; }  // Définir explicitement la clé étrangère AgentId
        public Agent Agent { get; set; }
        public decimal MontantPaye { get; set; }
        public DateTime DatePaiement { get; set; }
        public string ModeDePaiement { get; set; } // Ex: "Virement", "Chèque"

        public void EffectuerPaiement()
        {
            // Logique pour effectuer le paiement
        }
    }
}
