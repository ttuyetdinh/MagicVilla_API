using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaAPI.Model.DTO
{
    public class VillaNumberUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        public int VillaRoom { get; set; }
        [Required]
        public int VillaId { get; set; } // foregin key 
        public string? SpecialDetails { get; set; }

    }
}