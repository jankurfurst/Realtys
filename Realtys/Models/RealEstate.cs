using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Realtys.Models
{
    /// <summary>
    /// Model nemovitosti.
    /// </summary>
    [Table(nameof(RealEstate))]
    public class RealEstate
    {
        /// <summary>
        /// ID nemovitosti. Primární klíč do databáze.
        /// </summary>
        [Key]
        [Required]
        public int ID { get; set; }

        /// <summary>
        /// Název nemovitosti, jak bude uložena v databázi.
        /// </summary>
        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Měsíční náklady na nemovitost.
        /// </summary>
        [Required]
        public int? MonthlyExpenses { get; set; }

        /// <summary>
        /// Mesíční nájem z nemovitosti.
        /// </summary>
        [Required]
        public int? MonthlyRent { get; set; }
        /// <summary>
        /// Cena nemovitosti.
        /// </summary>
        [Required]
        public int? RealtyPrice { get; set; }

        /// <summary>
        /// Neobsazenost nemovitosti za první rok v %.
        /// </summary>
        [Required]
        public double? Vacancy { get; set; }

        /// <summary>
        /// Na jak dlouho bude nemovitost držena.
        /// </summary>
        [Required]
        public int? ForYears { get; set; }

        /// <summary>
        /// Použití úvěru.
        /// True pokud je úvěr použit.
        /// False pokud není úvěr použit.
        /// </summary>
        [Required]
        public bool MortgageUsage { get; set; }
    }
}
