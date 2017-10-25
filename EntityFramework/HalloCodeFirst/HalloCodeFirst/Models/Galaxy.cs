using System;

namespace HalloCodeFirst.Models
{
    internal class Galaxy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DiscoveryDate { get; set; }
        public GalaxyForm Form { get; set; }
    }
}
