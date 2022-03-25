using Realtys.Models;

namespace Realtys.Database
{
    public class DatabaseInit
    {
        public void Initialization(RealtysDbContext realtysDbContext)
        {
            
            var can = realtysDbContext.Database.CanConnect();

            if (realtysDbContext.RealEstates.Count() == 0)
            {
                IList<RealEstate> REs = GenerateRealEstates();
                foreach (var re in REs)
                {
                    realtysDbContext.RealEstates.Add(re);
                }
                realtysDbContext.SaveChanges();
            }
        }
        public List<Mortgage> GenerateMortgages()
        {
            List<Mortgage> mortgages = new List<Mortgage>();

            Mortgage p1 = new Mortgage()
            {
                ID = 0

            };
            Mortgage p2 = new Mortgage()
            {
                ID = 1

            };


            mortgages.Add(p1);
            mortgages.Add(p2);

            return mortgages;
        }


        public List<RealEstate> GenerateRealEstates()
        {
            List<RealEstate> realEstates = new List<RealEstate>();

            RealEstate ci1 = new RealEstate()
            {
                
                Nazev = "TEST1",
                mesicniNaklady = 1000,
                mesicniNajem = 5000,
                cenaNemovitosti = 1000000,
                neobsazenost = 0


            };
            RealEstate ci2 = new RealEstate()
            {
                
                Nazev = "TEST2",
                mesicniNaklady = 2000,
                mesicniNajem = 7000,
                cenaNemovitosti = 3000000,
                neobsazenost = 2

            };


            realEstates.Add(ci1);
            realEstates.Add(ci2);


            return realEstates;
        }


    }
}
