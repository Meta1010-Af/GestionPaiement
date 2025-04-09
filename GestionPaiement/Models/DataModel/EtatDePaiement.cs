using System.ComponentModel.DataAnnotations;

namespace GestionPaiement.Models.DataModel
{
    public class EtatDePaiement
    {
        [Key]
        public int IdEtat { get; set; }
        public int AgentId { get; set; }  // Définir explicitement la clé étrangère AgentId
        public Agent Agent { get; set; }  // Navigation vers Agent
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public decimal TotalPaye { get; set; }
        public List<Paiement> Paiements { get; set; } = new List<Paiement>();

        // Méthode pour générer l'état de paiement
        public void GenererEtat()
        {
            TotalPaye = Paiements.Sum(p => p.MontantPaye);
        }
    }
}
