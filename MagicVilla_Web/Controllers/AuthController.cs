using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AutoMapper;
using MagicVilla_Ultility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static MagicVilla_Ultility.SD;

namespace MagicVilla_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authServices;
        private readonly IMapper _mapper;

        public AuthController(ILogger<AuthController> logger, IAuthService authServices, IMapper mapper)
        {
            _logger = logger;
            _authServices = authServices;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO obj = new LoginRequestDTO();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            var response = await _authServices.LoginAsync<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(model.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim( new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u=>u.Type == "unique_name").Value));
                identity.AddClaim( new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u=>u.Type == "role").Value));
                
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal); 

                HttpContext.Session.SetString(SD.SessionToken, model.Token);
                
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("CustomeError", response.ErrorMessage.FirstOrDefault());
            return View(obj);
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterationRequestDTO obj = new RegisterationRequestDTO(
            ){
                // Name="hello from here",
                // UserName ="hi"
            };
            
            var roleList = new List<SelectListItem>();
            foreach(var role in Enum.GetValues(typeof(Role))){
                roleList.Add(new SelectListItem(){Text = role.ToString(), Value = role.ToString()});
            }
            ViewBag.RoleList = roleList;
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDTO obj)
        {   
            obj.Role = obj.Role == null ? Role.User : obj.Role;
            var result = await _authServices.RegisterAsync<APIResponse>(obj);

            if (result != null && result.IsSuccess) return RedirectToAction("login");

            var roleList = new List<SelectListItem>();
            foreach(var role in Enum.GetValues(typeof(Role))){
                roleList.Add(new SelectListItem(){Text = role.ToString(), Value = role.ToString()});
            }
            ViewBag.RoleList = roleList;

            return View(obj);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}