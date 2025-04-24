using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

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

        // Propriétés pour les relations avec d'autres entités
        public virtual List<BulletinDeSalaire> BulletinDeSalaires { get; set; }
        public virtual List<EtatDePaiement> EtatDePaiements { get; set; }
        public virtual List<Paiement> Paiements { get; set; }
        public virtual List<Salaire> Salaires { get; set; }
        public List<Rubrique> Rubriques { get; set; } = new List<Rubrique>();

        // Propriété pour afficher le salaire brut formaté en FCFA
        [NotMapped]  // Indique que cette propriété ne doit pas être mappée à une colonne dans la base de données
        public string SalaireBrutFormatted
        {
            get
            {
                return SalaireBrut.ToString("C", new CultureInfo("fr-FR")) + " FCFA";
            }
        }

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
