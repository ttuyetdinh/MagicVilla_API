using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaAPI.Model
{
    public class Villa
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Details { get; set; }
        public double? Rate { get; set; }
        public int? Sqft { get; set; }
        public int? Occupancy { get; set; }
        public string? Amenity { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public int? Age { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<VillaNumber>? VillaNumbers { get; set; } // navigation key

    }
}