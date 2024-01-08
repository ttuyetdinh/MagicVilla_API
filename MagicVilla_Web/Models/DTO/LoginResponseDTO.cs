using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_Web.Models.DTO
{
    public class LoginResponseDTO
    {
        public LocalUserDTO? localUser { get; set; }
        public string? Token { get; set; }
    }
}