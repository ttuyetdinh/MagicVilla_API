using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_VillaAPI.Model.DTO;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO> {
                new VillaDTO{Id = 1, Name = "BaoThu", Description = "meet in Tra Vinh park", Age = 23},
                new VillaDTO{Id = 2, Name = "BaoTran", Description = "meet in English class", Age = 23}
        };
    }
}