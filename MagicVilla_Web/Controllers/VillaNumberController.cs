using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_Ultility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ILogger<VillaNumberController> _logger;
        private readonly IMapper _mapper;
        private readonly IVillaNumberServices _villaNumberServices;
        private readonly IVillaServices _villaServices;
        public VillaNumberController(ILogger<VillaNumberController> logger, IMapper mapper, IVillaNumberServices villaNumberServices, IVillaServices villaServices)
        {
            _logger = logger;
            _mapper = mapper;
            _villaNumberServices = villaNumberServices;
            _villaServices = villaServices;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO>? list = new();
            var AccessToken = HttpContext.Session.GetString(SD.AccessToken);
            var response = await _villaNumberServices.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }

            return View(list);

        }

        [Authorize(Roles = nameof(SD.Role.Admin))]
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villaNumberVM = new VillaNumberCreateVM();
            var AccessToken = HttpContext.Session.GetString(SD.AccessToken);
            var response = await _villaServices.GetAllAsync<APIResponse>();
            try
            {
                if (response != null && response.IsSuccess)
                {
                    villaNumberVM.VillaList = JsonConvert
                        .DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                        .Select(i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        });
                }
            }
            catch (Exception e)
            {
                villaNumberVM.VillaList = new List<SelectListItem>();
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(SD.Role.Admin))]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {
            var AccessToken = HttpContext.Session.GetString(SD.AccessToken);
            if (ModelState.IsValid)
            {
                var response = await _villaNumberServices.CreateAsync<APIResponse>(model.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    TempData["message"] = (response.ErrorMessage != null && response.ErrorMessage.Count > 0)
                    ? response.ErrorMessage[0]
                    : "Fail to create villa number";
                }
            }

            // Repopulates the villaList again

            var res = await _villaServices.GetAllAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {
                model.VillaList = JsonConvert
                    .DeserializeObject<List<VillaDTO>>(Convert.ToString(res.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            return View(model);
        }

        [Authorize(Roles = nameof(SD.Role.Admin))]
        public async Task<IActionResult> UpdateVillaNumber(int Id)
        {
            var villaNumberVM = new VillaNumberUpdateVM();
            var AccessToken = HttpContext.Session.GetString(SD.AccessToken);
            var response = await _villaNumberServices.GetAsync<APIResponse>(Id);
            if (response == null || response.IsSuccess == false) return NotFound();

            VillaNumberDTO villaNumberDTO = JsonConvert
                .DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            villaNumberVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(villaNumberDTO);


            response = await _villaServices.GetAllAsync<APIResponse>();
            try
            {
                if (response != null && response.IsSuccess)
                {
                    villaNumberVM.VillaList = JsonConvert
                        .DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                        .Select(i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        });
                }
                else
                {
                    TempData["message"] = (response.ErrorMessage != null && response.ErrorMessage.Count > 0)
                    ? response.ErrorMessage[0]
                    : "Fail to retrieve villa number";
                }
            }
            catch (Exception e)
            {
                villaNumberVM.VillaList = new List<SelectListItem>();
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(SD.Role.Admin))]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            var AccessToken = HttpContext.Session.GetString(SD.AccessToken);
            if (ModelState.IsValid)
            {
                var response = await _villaNumberServices.UpdateAsync<APIResponse>(model.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    TempData["message"] = (response.ErrorMessage != null && response.ErrorMessage.Count > 0)
                    ? response.ErrorMessage[0]
                    : "Fail to update villa number";
                }
            }

            // Repopulates the villaList again
            var res = await _villaServices.GetAllAsync<APIResponse>();
            if (res != null && res.IsSuccess)
            {
                model.VillaList = JsonConvert
                    .DeserializeObject<List<VillaDTO>>(Convert.ToString(res.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(SD.Role.Admin))]
        public async Task<IActionResult> DeleteVillaNumber(int Id)
        {
            var AccessToken = HttpContext.Session.GetString(SD.AccessToken);
            var response = await _villaNumberServices.DeleteAsync<APIResponse>(Id);


            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            else
            {
                TempData["message"] = (response.ErrorMessage != null && response.ErrorMessage.Count > 0)
                ? response.ErrorMessage[0]
                : "Fail to delete villa number";
            }

            return View();
        }


    }
}