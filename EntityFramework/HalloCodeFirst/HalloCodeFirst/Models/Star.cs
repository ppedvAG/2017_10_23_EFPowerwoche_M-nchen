﻿using System;

namespace HalloCodeFirst.Models
{
    public class Star
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DiscoveryDate { get; set; }
        public decimal Mass { get; set; }
        public float DistanceToEarth { get; set; }
        public string Description { get; set; }

        //[ForeignKey("Galaxy")]
        public int GalaxyId { get; set; }
        public virtual Galaxy Galaxy { get; set; }
    }
}
