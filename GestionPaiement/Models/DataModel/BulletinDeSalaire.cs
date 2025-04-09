using System.ComponentModel.DataAnnotations;

namespace GestionPaiement.Models.DataModel
{
    public class BulletinDeSalaire
    {
        [Key]
        public int IdBulletin { get; set; }
        public int AgentId { get; set; } // Ajouter un IdAgent pour la clé étrangère
        public Agent Agent { get; set; } // Propriété de navigation

        public DateTime Date { get; set; }
        public decimal SalaireBrut { get; set; }
        public decimal SalaireNet { get; set; }

        public void GenererBulletin()
        {
            SalaireBrut = Agent.SalaireBrut;
            SalaireNet = Agent.CalculerSalaireNet();
        }
    }
}
