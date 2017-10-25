using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloCodeFirst.Models
{
    internal class Galaxy
    {
        [Key]
        [Column("GalaxyId")]
        public int Id { get; set; }

        //[Key]
        //[Column("GalaxyId2", Order = 2)]
        //public int Id2 { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime DiscoveryDate { get; set; }

        [Column("GalaxyForm")]
        public GalaxyForm Form { get; set; }
    }
}
