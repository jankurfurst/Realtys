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
                var realty = realtysDbContext.RealEstates.FirstOrDefault(re => re.RealtyPrice == 3000000);
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
            double urok = 4;//%
            double podil = 80.0/100.0;
            double pocatecniDluh = r.RealtyPrice * (1 - podil);
            int pocetLet = 30;

            int pocetMesicu = pocetLet * 12;
            double i = urok / 100;

            double v = 1 / (1 + i);
            double splatka = ((i * pocatecniDluh) / (1 - Math.Pow(v, pocetMesicu)));


            List<Mortgage> mortgages = new List<Mortgage>();

            Mortgage m = new Mortgage()
            {
                MonthlyInterest = urok,
                Share = podil,
                InitialDebt = pocatecniDluh,
                ForYears = pocetLet,
                Payment = splatka,
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
                Name = "TEST1",
                MonthlyExpenses = 1000,
                MonthlyRent = 5000,
                RealtyPrice = 1000000,
                Vacancy = 0,
                MortgageUsage = false


            };
            RealEstate r2 = new RealEstate()
            {
                Name = "TEST2",
                MonthlyExpenses = 2000,
                MonthlyRent = 7000,
                RealtyPrice = 3000000,
                Vacancy = 2,
                MortgageUsage = true

            };


            realEstates.Add(r1);
            realEstates.Add(r2);


            return realEstates;
        }


    }
}
