using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_Web.Models.DTO
{
    public class TokenDTO
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}