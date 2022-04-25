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
        [NotMapped]
        private int? monthlyExpenses;
        [NotMapped]
        private int? monthlyRent;
        [NotMapped]
        private int? realtyPrice;
        [NotMapped]
        private double? vacancy;
        [NotMapped]
        private int? forYears;

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
        public string MonthlyExpenses
        {
            get => monthlyExpenses.ToString();
            set
            {
                int i;
                if (Int32.TryParse(value, out i)) monthlyExpenses = i;
                else monthlyExpenses = null;
            }
        }

        /// <summary>
        /// Mesíční nájem z nemovitosti.
        /// </summary>
        [Required]
        public string MonthlyRent
        {
            get => monthlyRent.ToString();
            set
            {
                int i;
                if (Int32.TryParse(value, out i)) monthlyRent = i;
                else monthlyRent = null;
            }
        }
        /// <summary>
        /// Cena nemovitosti.
        /// </summary>
        [Required]
        public string RealtyPrice
        {
            get => realtyPrice.ToString();
            set
            {
                int i;
                if (Int32.TryParse(value, out i)) realtyPrice = i;
                else realtyPrice = null;
            }
        }

        /// <summary>
        /// Neobsazenost nemovitosti za první rok v %.
        /// </summary>
        [Required]
        public string Vacancy
        {
            get => vacancy.ToString();
            set
            {
                double i;
                if (Double.TryParse(value, out i)) vacancy = i;
                else vacancy = null;
            }
        }

        /// <summary>
        /// Na jak dlouho bude nemovitost držena.
        /// </summary>
        [Required]
        public string ForYears
        {
            get => forYears.ToString();
            set
            {
                int i;
                if (Int32.TryParse(value, out i)) forYears = i;
                else forYears = null;
            }
        }

        /// <summary>
        /// Použití úvěru.
        /// True pokud je úvěr použit.
        /// False pokud není úvěr použit.
        /// </summary>
        [Required]
        public bool MortgageUsage { get; set; }


    }
}
