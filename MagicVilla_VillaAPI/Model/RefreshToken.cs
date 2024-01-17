using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicVilla_VillaAPI.Model
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? JwtTokenId { get; set; }
        public string? Refresh_Token { get; set; }
        // refresh token is only valid for one use
        public bool? IsValid { get; set; }
        public DateTime? ExpriesAt { get; set; }



    }
}