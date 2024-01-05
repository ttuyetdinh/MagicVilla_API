using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaAPI.Model
{
    public class LocalUser
    {
        
        public int Id { get; set; }
        [Required]
        public string? UserName { get; set; }
        public string? Name { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public int? Role { get; set; }

    }
}