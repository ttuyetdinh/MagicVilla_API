using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaAPI.Model.DTO
{

    public class VillaDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        public string? Details { get; set; }

        [Required]
        public double? Rate { get; set; }

        public int? Sqft { get; set; }

        public int? Occupancy { get; set; }

        public string? Amenity { get; set; }

        public string? ImageUrl { get; set; }
        // public string? ImageLocalPath { get; set; }
        
        public int? Age { get; set; }
    };
}