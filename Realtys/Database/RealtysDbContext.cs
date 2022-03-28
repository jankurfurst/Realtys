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

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RealEstate>().ToTable("RealEstates");
            modelBuilder.Entity<Mortgage>().ToTable("Mortgages");

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var name = entity.GetTableName();
            }

        }

    }
}

