using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_Ultility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaServices _villaServices;
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, IVillaServices villaServices, IMapper mapper)
        {
            _logger = logger;
            _villaServices = villaServices;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();
            var sessionToken = HttpContext.Session.GetString(SD.SessionToken);

            var response = await _villaServices.GetAllAsync<APIResponse>(sessionToken);
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }

            return View(list);

        }
        [Authorize(Roles = nameof(SD.Role.Admin))]
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = nameof(SD.Role.Admin))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var sessionToken = HttpContext.Session.GetString(SD.SessionToken);
                var response = await _villaServices.CreateAsync<APIResponse>(model, sessionToken);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            return View(model);
        }

        [Authorize(Roles = nameof(SD.Role.Admin))]
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var sessionToken = HttpContext.Session.GetString(SD.SessionToken);
            var response = await _villaServices.GetAsync<APIResponse>(villaId, sessionToken);
            if (response != null && response.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));

                return View(_mapper.Map<VillaUpdateDTO>(model));
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = nameof(SD.Role.Admin))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var sessionToken = HttpContext.Session.GetString(SD.SessionToken);
                var response = await _villaServices.UpdateAsync<APIResponse>(model, sessionToken);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = nameof(SD.Role.Admin))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            if (ModelState.IsValid)
            {
                var sessionToken = HttpContext.Session.GetString(SD.SessionToken);
                var response = await _villaServices.DeleteAsync<APIResponse>(villaId, sessionToken);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            return View();
        }

    }
}