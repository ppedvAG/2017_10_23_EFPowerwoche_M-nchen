namespace HalloCodeFirst.Models
{
    public class Star : CelestialBody
    {
        public decimal Mass { get; set; }
        public float DistanceToEarth { get; set; }

        public int GalaxyId { get; set; }
        public Galaxy Galaxy { get; set; }
    }
}
