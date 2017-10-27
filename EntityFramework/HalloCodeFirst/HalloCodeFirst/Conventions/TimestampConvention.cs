using System.Data.Entity.ModelConfiguration.Conventions;

namespace HalloCodeFirst.Conventions
{
    internal class TimestampConvention : Convention
    {
        public TimestampConvention()
        {
            Properties<byte[]>()
                .Where(p => p.Name.ToLowerInvariant() == "timestamp")
                .Configure(c => c.IsRowVersion());
        }
    }
}
