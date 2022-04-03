using Microsoft.EntityFrameworkCore;
using Realtys.Models;

namespace Realtys.Database
{
    public class DatabaseInit
    {


        public void Initialization(RealtysDbContext realtysDbContext)
        {
            if (realtysDbContext.RealEstates.Count() == 0)
            {
                IList<RealEstate> REs = GenerateRealEstates();
                foreach (var re in REs)
                {
                    realtysDbContext.RealEstates.Add(re);
                }
                realtysDbContext.SaveChanges();
            }

            if (realtysDbContext.Mortgages.Count() == 0)
            {
                var realty = realtysDbContext.RealEstates.FirstOrDefault(re => re.cenaNemovitosti == 3000000);
                IList<Mortgage> mortgages = GenerateMortgages(realty);
                foreach (var m in mortgages)
                {
                    realtysDbContext.Mortgages.Add(m);
                }
                realtysDbContext.SaveChanges();
            }
        }
        public List<Mortgage> GenerateMortgages(RealEstate r)
        {
            double urok = 10;//%
            double podil = 70.0/100.0;
            double pocatecniDluh = r.cenaNemovitosti * (1 - podil);
            int pocetLet = 30;

            int pocetMesicu = pocetLet * 12;
            double i = urok / 100;

            double v = 1 / (1 + i);
            double splatka = ((i * pocatecniDluh) / (1 - Math.Pow(v, pocetMesicu)));


            List<Mortgage> mortgages = new List<Mortgage>();

            Mortgage m = new Mortgage()
            {
                mesicniUrokovaMira = urok,
                podil = podil,
                pocatecniDluh = pocatecniDluh,
                pocetLet = pocetLet,
                splatka = splatka,
                RealtyID = r.ID
        };

            mortgages.Add(m);
            return mortgages;
        }


        public List<RealEstate> GenerateRealEstates()
        {
            List<RealEstate> realEstates = new List<RealEstate>();

            RealEstate r1 = new RealEstate()
            {
                Nazev = "TEST1",
                mesicniNaklady = 1000,
                mesicniNajem = 5000,
                cenaNemovitosti = 1000000,
                neobsazenost = 0,
                pouzitiHypo = false


            };
            RealEstate r2 = new RealEstate()
            {
                Nazev = "TEST2",
                mesicniNaklady = 2000,
                mesicniNajem = 7000,
                cenaNemovitosti = 3000000,
                neobsazenost = 2,
                pouzitiHypo = true

            };


            realEstates.Add(r1);
            realEstates.Add(r2);


            return realEstates;
        }


    }
}
