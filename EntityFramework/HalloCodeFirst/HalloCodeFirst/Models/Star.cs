using System;

namespace HalloCodeFirst.Models
{
    internal class Star
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DiscoveryDate { get; set; }
        public decimal Mass { get; set; }
        public float DistanceToEarth { get; set; }
        public string Description { get; set; }

        //[ForeignKey("Galaxy")]
        public int GalaxyId { get; set; }
        public Galaxy Galaxy { get; set; }
    }
}
