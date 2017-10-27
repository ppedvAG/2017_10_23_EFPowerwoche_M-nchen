using System;
using System.Collections.Generic;

namespace HalloCodeFirst.Models
{
    public class Galaxy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DiscoveryDate { get; set; }
        public GalaxyForm Form { get; set; }
        public string Description { get; set; }

        public ICollection<Star> Stars { get; } = new HashSet<Star>();
    }
}
