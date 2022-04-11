using Microsoft.EntityFrameworkCore;
using Realtys.Models;

namespace Realtys.Database
{
    public class RealtysDbContext : DbContext
    {
        public DbSet<RealEstate> RealEstates { get; set; }

        public DbSet<Mortgage> Mortgages { get; set; }


        public RealtysDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}

