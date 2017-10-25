﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloCodeFirst.Models
{
    [Table("Galaxies_Table")]
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

        //[NotMapped]
        [Column("GalaxyForm")]
        public GalaxyForm Form { get; set; }

        public string Description { get; set; }

        public ICollection<Star> Stars { get; } = new HashSet<Star>();
    }
}
