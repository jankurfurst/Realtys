using Microsoft.EntityFrameworkCore;
using Realtys.Models;

namespace Realtys.Database
{
    public class DatabaseInit
    {


        public void Initialization(RealtysDbContext realtysDbContext)
        {
            if (!realtysDbContext.RealEstates.Any())
            {
                IList<RealEstate> REs = DatabaseInit.GenerateRealEstates();
                foreach (var re in REs)
                {
                    realtysDbContext.RealEstates.Add(re);
                }
                realtysDbContext.SaveChanges();

                if (!realtysDbContext.Mortgages.Any())
                {
                    var realty = realtysDbContext.RealEstates.FirstOrDefault(re => re.ID == 2);
                    IList<Mortgage> mortgages = GenerateMortgages(realty);
                    foreach (var m in mortgages)
                    {
                        realtysDbContext.Mortgages.Add(m);
                    }
                    realtysDbContext.SaveChanges();
                }
            }

        }
        public List<Mortgage> GenerateMortgages(RealEstate r)
        {
            double urok_sazba = 3;//%
            double podil = 80.0;
            double pocatecniDluh = (double)r.RealtyPrice * (podil / 100.0);
            int pocetLet = 30;

            int pocetMesicu = pocetLet * 12;
            double urokova_mira = (urok_sazba / 100) / 12;

            double v = 1 / (1 + urokova_mira);
            double splatka = ((urokova_mira * pocatecniDluh) / (1 - Math.Pow(v, pocetMesicu)));


            List<Mortgage> mortgages = new();

            Mortgage m = new()
            {
                Interest = (int?)urok_sazba,
                Share = podil,
                InitialDebt = pocatecniDluh,
                ForYears = pocetLet,
                Payment = splatka,
                RealtyID = r.ID
        };

            mortgages.Add(m);
            return mortgages;
        }


        public static List<RealEstate> GenerateRealEstates()
        {
            List<RealEstate> realEstates = new();

            var naklady = 1227;
            var najem = 7400;
            var cena = 1600000;
            var neobsazenost = 3;
            var pocetLet = 20;

            RealEstate r1 = new()
            {
                Name = "Chata - zelená střecha na svahu",
                MonthlyExpenses = naklady,
                MonthlyRent = najem,
                RealtyPrice = cena,
                Vacancy = neobsazenost,
                ForYears = pocetLet,
                MortgageUsage = false


            };
            RealEstate r2 = new()
            {
                Name = "Byt - ulice Mokrá 773, Praha",
                MonthlyExpenses = naklady,
                MonthlyRent = najem,
                RealtyPrice = cena,
                Vacancy = neobsazenost,
                ForYears= pocetLet,
                MortgageUsage = true

            };


            realEstates.Add(r1);
            realEstates.Add(r2);


            return realEstates;
        }


    }
}
