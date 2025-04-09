using System.ComponentModel.DataAnnotations;

namespace GestionPaiement.Models.DataModel
{
    public class Agent
    {
        [Key]
        public int IdAgent { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public DateTime DateEmbauche { get; set; }
        public string Poste { get; set; }
        public decimal SalaireBrut { get; set; }
        public virtual List<BulletinDeSalaire> BulletinDeSalaires { get; set; }
        public virtual List<EtatDePaiement> EtatDePaiements { get; set; }
        public virtual List<Paiement> Paiements { get; set; }
        public virtual List<Salaire> Salaires { get; set; }
        public List<Rubrique> Rubriques { get; set; } = new List<Rubrique>();

        // Méthode pour calculer le salaire net
        public decimal CalculerSalaireNet()
        {
            decimal totalAvantages = 0;
            decimal totalRetenues = 0;

            foreach (var rubrique in Rubriques)
            {
                if (rubrique.Type == "Avantage")
                    totalAvantages += rubrique.Montant;
                else if (rubrique.Type == "Retenue")
                    totalRetenues += rubrique.Montant;
            }

            return SalaireBrut + totalAvantages - totalRetenues;
        }
    }
}
