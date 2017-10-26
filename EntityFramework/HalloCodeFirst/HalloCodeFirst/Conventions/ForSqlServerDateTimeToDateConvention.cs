using System;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HalloCodeFirst.Conventions
{
    internal class ForSqlServerDateTimeToDateConvention : Convention
    {
        public ForSqlServerDateTimeToDateConvention()
        {
            Properties<DateTime>()
                .Configure(p => p.HasColumnType("date"));
        }
    }
}
