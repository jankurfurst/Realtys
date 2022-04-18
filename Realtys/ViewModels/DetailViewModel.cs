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


        private int mesicniNakladySHypo
        {
            get
            {
                if (mortgage != null) return (int)mortgage.Payment;
                return 0;                
            }
        }
        
        public int cistyRocniVynos => (int)((realEstate.MonthlyRent - mesicniNakladySHypo - realEstate.MonthlyExpenses) * (12 - realEstate.Vacancy));
        
        public int porizovaciCena
        {
            get
            {
                if (mortgage == null) return (int)realEstate.RealtyPrice;
                return (int)(realEstate.RealtyPrice - Math.Floor(mortgage.InitialDebt));
            }
        }

        
        public int HrubyVynos => ((int)((realEstate.MonthlyRent * 12) / porizovaciCena));

        
        public double RocniNavratnost => (porizovaciCena / cistyRocniVynos);


        public double CistyVynos { get
            {
                if(cistyRocniVynos == 0) return 0;
                return (porizovaciCena / cistyRocniVynos);
            } 
        } 


        /*
        public double Rentabilita()
        {
            return (cistyRocniVynos / porizovaciCena);
        }*/




    }
}
