using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaAPI.Model.DTO
{
    public class LoginResponseDTO
    {
        public UserDTO? ApplicationUser { get; set; }
        public string Role { get; set; }
        public string? Token { get; set; }
    }
}