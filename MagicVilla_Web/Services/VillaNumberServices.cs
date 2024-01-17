using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_Ultility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaNumberServices : IVillaNumberServices
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseServices _baseServices;
        private string villaUrl;
        public VillaNumberServices(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseServices baseServices)
        {
            _clientFactory = clientFactory;
            _baseServices = baseServices;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaUrl");
        }
        public async Task<T> CreateAsync<T>(VillaNumberCreateDTO dto)
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/villaNumberAPI",
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/villaNumberAPI/{id}",
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/villaNumberAPI",
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/villaNumberAPI/{id}",
            });
        }

        public async Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto)
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/villaNumberAPI/{dto.Id}",
            });
        }
    }
}