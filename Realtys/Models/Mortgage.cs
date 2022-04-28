using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realtys.Models
{
    /// <summary>
    /// Model hypotečního úvěru.
    /// </summary>
    [Table(nameof(Mortgage))]
    public class Mortgage
    {
        [NotMapped]
        private int? forYears;
        [NotMapped]
        private double? share;
        [NotMapped]
        private double? interest;

        /// <summary>
        /// ID úvěru.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Roční úroková sazba úvěru.
        /// </summary>
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

        /// <summary>
        /// Podíl z ceny nemovitsti, pokrytý úvěrem
        /// </summary>
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

        /// <summary>
        /// Počáteční dluh.
        /// </summary>
        [Required]
        public double InitialDebt { get; set; }
        
        /// <summary>
        /// Počet let splatnosti úvěru.
        /// </summary>
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

        /// <summary>
        /// Splátka úvěru.
        /// </summary>
        [Required]
        public double Payment { get; set; }

        /// <summary>
        /// ID nemovitosti, ke které je úvěr vázán.
        /// </summary>
        [ForeignKey(nameof(RealEstate))]
        public int RealtyID { get; set; }

    }
}