using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static MagicVilla_Ultility.SD;

namespace MagicVilla_Web.Models
{
    public class APIRequest
    {
        public ApiType ApiType{ get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }

        public ContentType ContentType { get; set; } = ContentType.Json;
        
        
    }
}