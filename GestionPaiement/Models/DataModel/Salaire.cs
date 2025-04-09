using System.ComponentModel.DataAnnotations;

namespace GestionPaiement.Models.DataModel
{
    public class Salaire
    {
        [Key]
        public int IdSalaire { get; set; }
        public int AgentId { get; set; } // Ajouter un IdAgent pour la clé étrangère
        public Agent Agent { get; set; }
        public decimal SalaireBrut { get; set; }
        public decimal SalaireNet { get; set; }
        public DateTime DateDePaie { get; set; }

        public void CalculerSalaireNet()
        {
            SalaireNet = Agent.CalculerSalaireNet();
        }
    }
}
