using HalloCodeFirst.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

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
            modelBuilder.Conventions.Add<Conventions.StringConventions>();
            modelBuilder.Conventions.Add<Conventions.ForSqlServerDateTimeToDateConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new Configurations.StarConfiguration());

            modelBuilder.Entity<Galaxy>()
                .HasMany(g => g.Stars)
                .WithRequired(s => s.Galaxy)
                .HasForeignKey(s => s.GalaxyId)
                .WillCascadeOnDelete(true);
            // entweder ↑ oder ↓
            //modelBuilder.Entity<Star>()
            //    .HasRequired(s => s.Galaxy)
            //    .WithMany(g => g.Stars)
            //    .HasForeignKey(s => s.GalaxyId)
            //    .WillCascadeOnDelete(true);
        }
    }
}
