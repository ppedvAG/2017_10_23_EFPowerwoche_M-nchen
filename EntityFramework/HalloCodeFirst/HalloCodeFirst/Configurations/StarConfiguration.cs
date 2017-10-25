using HalloCodeFirst.Models;
using System.Data.Entity.ModelConfiguration;

namespace HalloCodeFirst.Configurations
{
    internal class StarConfiguration : EntityTypeConfiguration<Star>
    {
        public StarConfiguration()
        {
            ToTable("Stars_Table");

            HasKey(s => s.Id);

            Property(s => s.Id)
                .HasColumnName("StarId");

            Property(s => s.DiscoveryDate)
                .HasColumnType("date");

            //modelBuilder.Entity<Star>().HasKey(s => new { s.Id, s.Id2, s.Id3 });
            //modelBuilder.Entity<Star>().Ignore(s => s.Mass);
        }
    }
}
