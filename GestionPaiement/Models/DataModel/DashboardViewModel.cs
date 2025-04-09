using System.ComponentModel.DataAnnotations;

namespace GestionPaiement.Models.DataModel
{
    public class DashboardViewModel
    {
        [Key]
        public int TotalAgents { get; set; }
        public decimal TotalSalaires { get; set; }
        public int TotalPaiements { get; set; }
        public int TotalBulletinsDeSalaire { get; set; }
        public int TotalRubriques { get; set; }
        public int TotalEtatDePaiement { get; set; }
    }
}
