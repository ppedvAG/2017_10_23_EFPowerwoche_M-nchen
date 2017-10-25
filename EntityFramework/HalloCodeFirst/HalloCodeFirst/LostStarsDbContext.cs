using System.Data.Entity;

namespace HalloCodeFirst
{
    internal class LostStarsDbContext : DbContext
    {
        public LostStarsDbContext() : base("name=LostStarConnectionString")
        { }
        public LostStarsDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        { }
    }
}
