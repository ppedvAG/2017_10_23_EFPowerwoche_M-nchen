using HalloCodeFirst.Models;
using System.Data.Entity;

namespace HalloCodeFirst
{
    internal class LostStarsDbContext : DbContext
    {
        public LostStarsDbContext() : base("name=LostStarConnectionString")
        { }
        public LostStarsDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        { }

        public DbSet<Galaxy> Galaxies { get; set; }
        public DbSet<Star> Stars { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Star>().HasKey(s => s.Id);
            //modelBuilder.Entity<Star>().HasKey(s => new { s.Id, s.Id2, s.Id3 });
            modelBuilder.Entity<Star>().Property(s => s.Id).HasColumnName("StarId");
            modelBuilder.Entity<Star>().Property(s => s.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Star>().Property(s => s.DiscoveryDate).HasColumnType("date");
            //modelBuilder.Entity<Star>().Ignore(s => s.Mass);
        }
    }
}
