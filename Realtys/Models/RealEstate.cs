using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Realtys.Models
{
    [Table(nameof(RealEstate))]
    public class RealEstate
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        [Required]
        public int MonthlyExpenses { get; set; }

        [Required]
        public int MonthlyRent { get; set; }

        [Required]
        public int RealtyPrice { get; set; }

        [Required]
        public int Vacancy { get; set; }

        [Required]
        public bool MortgageUsage { get; set; }
    }
}
