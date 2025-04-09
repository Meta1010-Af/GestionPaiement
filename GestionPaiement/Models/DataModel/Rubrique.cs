using System.ComponentModel.DataAnnotations;

namespace GestionPaiement.Models.DataModel
{
    public class Rubrique
    {
        [Key]
        public int IdRubrique { get; set; }
        public string Nom { get; set; }
        public decimal Montant { get; set; }
        public string Type { get; set; } // "Avantage" ou "Retenue"
    }
}
