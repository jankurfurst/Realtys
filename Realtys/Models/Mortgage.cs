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
        /// <summary>
        /// ID úvěru.
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Roční úroková sazba úvěru.
        /// </summary>
        [Required]
        public double? Interest { get; set; }

        /// <summary>
        /// Podíl z ceny nemovitosti, pokrytý úvěrem
        /// </summary>
        [Required]
        public double? Share { get; set; }

        /// <summary>
        /// Počáteční dluh.
        /// </summary>
        [Required]
        public double InitialDebt { get; set; }
        
        /// <summary>
        /// Počet let splatnosti úvěru.
        /// </summary>
        [Required]
        public int? ForYears { get; set; }

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