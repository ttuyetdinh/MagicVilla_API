using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_Ultility;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseServices _baseServices;
        private string villaUrl;
        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseServices baseServices)
        {
            _clientFactory = clientFactory;
            _baseServices = baseServices;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaUrl");
        }
        public async Task<T> LoginAsync<T>(LoginRequestDTO obj)
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/UsersAuth/login"

            }, withBearer: false);
        }

        public async Task<T> RegisterAsync<T>(RegisterationRequestDTO obj)
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/UsersAuth/register"

            }, withBearer: false);
        }

        public async Task<T> LogoutAsync<T>(TokenDTO tokenDTO)
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = tokenDTO,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/UsersAuth/revoke"

            }, withBearer: true);
        }
    }
}