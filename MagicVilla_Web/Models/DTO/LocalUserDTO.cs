using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using static MagicVilla_Ultility.SD;

namespace MagicVilla_Web.Models.DTO
{
    public class LocalUserDTO
    {
        
        public int Id { get; set; }
        [Required]
        public string? UserName { get; set; }
        public string? Name { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public Role? Role { get; set; }

    }
}