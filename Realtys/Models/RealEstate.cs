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

        //public int hypoID { get; set; }

        //[NotMapped]
        //public Mortgage hypo {
        //    get 
        //    {
        //        if (hypoID == null) return null;
        //        else
        //        {
        //            Mortgage mortgage = App.Database.GetMortgageAsync(hypoID);
        //            return mortgage;
        //        }
        //    }
        //}


        [NotMapped]
        public int mesicniNakladySHypo;
        [NotMapped]
        public int cistyRocniVynos => (mesicniNajem - mesicniNakladySHypo) * (12 - neobsazenost);
        [NotMapped]
        public int porizovaciCena
        {
            get
            {
                return cenaNemovitosti;

                //pouzit pri pripojeni hypoteky
                //if (hypo == null) return cenaNemovitosti;
                //return (int)(cenaNemovitosti - Math.Floor(hypo.pocatecniDluh));
            }
        }

        [NotMapped]
        public int HrubyVynos => ((mesicniNajem * 12) / porizovaciCena);

        [NotMapped]
        public double RocniNavratnost => (porizovaciCena / cistyRocniVynos);


        public int CistyVynos()
        {
            return (cistyRocniVynos / porizovaciCena);
        }

        /*
        public double Rentabilita()
        {
            return (cistyRocniVynos / porizovaciCena);
        }*/

    }
}
