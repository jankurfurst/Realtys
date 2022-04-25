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
                double i;
                if(Double.TryParse(value, out i)) interest = i;
                else interest = null;
            }
        }

        [Required]
        public string Share
        {
            get => share.ToString();
            set
            {
                double i;
                if (Double.TryParse(value, out i)) share = i;
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
                int i;
                if (Int32.TryParse(value, out i)) forYears = i;
                else forYears = null;
            }
        }
        [Required]
        public double Payment { get; set; }
        [ForeignKey(nameof(RealEstate))]
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