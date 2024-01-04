using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Model
{   
    public class VillaNumber
    {   
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }
        public string? SpecialDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [ForeignKey("Villa")]
        public int? VillaId { get; set; } // foregin key
        public Villa? Villa { get; set; } // navigation property

    }
}