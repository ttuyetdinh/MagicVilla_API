using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MagicVilla_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaServices _villaServices;
        private readonly IMapper _mapper;

        public HomeController(ILogger<VillaController> logger, IVillaServices villaServices, IMapper mapper)
        {
            _logger = logger;
            _villaServices = villaServices;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<VillaDTO> list = new();

            var response = await _villaServices.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }

            return View(list);

        }
    }
}