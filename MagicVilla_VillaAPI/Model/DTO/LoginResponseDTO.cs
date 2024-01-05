using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaAPI.Model.DTO
{
    public class LoginResponseDTO
    {
        public LocalUser? localUser { get; set; }
        public string? Token { get; set; }
    }
}