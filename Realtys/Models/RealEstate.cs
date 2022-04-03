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
        public string Nazev { get; set; }

        [Required]
        public int mesicniNaklady { get; set; }

        [Required]
        public int mesicniNajem { get; set; }

        [Required]
        public int cenaNemovitosti { get; set; }

        [Required]
        public int neobsazenost { get; set; }

        [Required]
        public bool pouzitiHypo { get; set; }
    }
}
