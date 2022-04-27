using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realtys.Models
{
    [Table(nameof(Mortgage))]
    public class Mortgage
    {
        [NotMapped]
        private int? forYears;
        [NotMapped]
        private double? share;
        [NotMapped]
        private double? interest;

        [Key]
        public int ID { get; set; }
        [Required]
        public string Interest
        {
            get => interest.ToString();
            set
            {
                if(Double.TryParse(value, out double i)) interest = i;
                else interest = null;
            }
        }

        [Required]
        public string Share
        {
            get => share.ToString();
            set
            {
                if (Double.TryParse(value, out double i)) share = i;
                else share = null;
            }
        }

        [Required]
        public double InitialDebt { get; set; }
        [Required]
        public string ForYears
        {
            get => forYears.ToString();
            set
            {
                if (Int32.TryParse(value, out int i)) forYears = i;
                else forYears = null;
            }
        }
        [Required]
        public double Payment { get; set; }
        [ForeignKey(nameof(RealEstate))]
        public int RealtyID { get; set; }

    }
}