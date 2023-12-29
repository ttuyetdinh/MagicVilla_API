using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {

            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO,VillaUpdateDTO>().ReverseMap();

            CreateMap<VillaNumberDTO,VillaNumberUpdateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();

        }
    }
}