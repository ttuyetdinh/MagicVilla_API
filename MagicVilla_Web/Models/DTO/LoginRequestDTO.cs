using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_Web.Models.DTO
{
    public class LoginRequestDTO
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}