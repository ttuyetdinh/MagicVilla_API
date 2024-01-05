using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MagicVilla_Ultility.SD;

namespace MagicVilla_VillaAPI.Model.DTO
{
    public class RegisterationRequestDTO
    {
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public Role? Role { get; set; }
        
    }
}