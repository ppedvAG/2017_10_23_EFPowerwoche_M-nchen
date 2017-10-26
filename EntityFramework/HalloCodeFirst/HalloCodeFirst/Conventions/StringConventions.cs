using System.Data.Entity.ModelConfiguration.Conventions;

namespace HalloCodeFirst.Conventions
{
    internal class StringConventions : Convention
    {
        public StringConventions()
        {
            Properties<string>()
                .Configure(c => c.IsRequired().HasMaxLength(80));

            Properties<string>()
                .Where(p => p.Name.ToLower(/* cultureInfo */).Contains("description"))
                .Configure(c => c.IsOptional().IsMaxLength());
        }
    }
}
