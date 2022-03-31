using Realtys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realtys.ViewModels
{
    public class DetailViewModel
    {
        public RealEstate realEstate { get; set; }
        public Mortgage mortgage { get; set; }


        public DetailViewModel(int id)
        {
            realEstate = App.DbContext.RealEstates.FirstOrDefault(r => r.ID == id);
            mortgage = App.DbContext.Mortgages.FirstOrDefault(m => m.RealtyID == id);
        }
        

        private int mesicniNakladySHypo;
        
        public int cistyRocniVynos => (realEstate.mesicniNajem - mesicniNakladySHypo) * (12 - realEstate.neobsazenost);
        
        public int porizovaciCena
        {
            get
            {
                //return realEstate.cenaNemovitosti;

                //pouzit pri pripojeni hypoteky
                if (mortgage == null) return realEstate.cenaNemovitosti;
                return (int)(realEstate.cenaNemovitosti - Math.Floor(mortgage.pocatecniDluh));
            }
        }

        
        public int HrubyVynos => ((realEstate.mesicniNajem * 12) / porizovaciCena);

        
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
