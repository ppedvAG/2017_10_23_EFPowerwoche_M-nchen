using System.Collections.Generic;

namespace HalloCodeFirst.Models
{
    public class Galaxy : CelestialBody
    {
        public GalaxyForm Form { get; set; }

        public ICollection<Star> Stars { get; } = new HashSet<Star>();
    }
}
