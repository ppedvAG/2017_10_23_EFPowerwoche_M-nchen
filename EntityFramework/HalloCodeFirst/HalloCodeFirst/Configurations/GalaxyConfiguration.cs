using HalloCodeFirst.Models;
using System.Data.Entity.ModelConfiguration;

namespace HalloCodeFirst.Configurations
{
    internal class GalaxyConfiguration : EntityTypeConfiguration<Galaxy>
    {
        public GalaxyConfiguration()
        {
            ToTable("Galaxies_Table");

            HasKey(g => g.Id);
            Property(g => g.Id)
                .HasColumnName("GalaxyId");

            Property(g => g.Form)
                .HasColumnName("GalaxyForm");

            HasMany(g => g.Stars)
               .WithRequired(s => s.Galaxy)
               .HasForeignKey(s => s.GalaxyId)
               .WillCascadeOnDelete(true);
        }
    }
}
