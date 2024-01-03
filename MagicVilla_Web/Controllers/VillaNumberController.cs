using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ILogger<VillaNumberController> _logger;
        private readonly IMapper _mapper;
        private readonly IVillaNumberServices _villaNumberServices;

        public VillaNumberController(ILogger<VillaNumberController> logger, IMapper mapper, IVillaNumberServices villaNumberServices)
        {
            _logger = logger;
            _mapper = mapper;
            _villaNumberServices =villaNumberServices;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();

            var response = await _villaNumberServices.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }

            return View(list);

        }    

    }
}