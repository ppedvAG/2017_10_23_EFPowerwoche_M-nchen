using System;

namespace HalloCodeFirst.Models
{
    public abstract class CelestialBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DiscoveryDate { get; set; }
        public string Description { get; set; }
    }
}
