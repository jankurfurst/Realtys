using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Realtys.Models
{
    [Table(nameof(Mortgage))]
    public class Mortgage
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public double mesicniUrokovaMira { get; set; }
        [Required]
        public double podil { get; set; }
        [Required]
        public double pocatecniDluh { get; set; }
        [Required]
        public int pocetLet { get; set; }
        [Required]
        public double splatka { get; set; }
        [Required]
        public int RealtyID { get; set; }

        //public Mortgage(double urok, double podil, int cena, int pocetLet)
        //{
        //    this.mesicniUrokovaMira = urok;
        //    this.podil = podil / 100;
        //    pocatecniDluh = cena * (1 - this.podil);
        //    this.pocetLet = pocetLet;

        //    int pocetMesicu = pocetLet * 12; // pocet mesicu splaceni
        //    double i = mesicniUrokovaMira / 100; // desetinne cislo

        //    double v = 1 / (1 + i);
        //    this.splatka = (i * pocatecniDluh) / (1 - Math.Pow(v, pocetMesicu));
        //}
    }
}