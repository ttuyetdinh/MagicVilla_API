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
    public class VillaServices : IVillaServices
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseServices _baseServices;
        private string villaUrl;
        public VillaServices(IHttpClientFactory clientFactory, IConfiguration configuration, IBaseServices baseServices)
        {
            _clientFactory = clientFactory;
            _baseServices = baseServices;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaUrl");
        }
        public async Task<T> CreateAsync<T>(VillaCreateDTO dto)
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/villaAPI",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<T> DeleteAsync<T>(int id)
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/villaAPI/{id}",
            });
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/villaAPI",
            });
        }

        public async Task<T> GetAsync<T>(int id)
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/villaAPI/{id}",
            });
        }

        public async Task<T> UpdateAsync<T>(VillaUpdateDTO dto)
        {
            return await _baseServices.SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = $"{villaUrl}/api/{SD.CurrentAPIVersion}/villaAPI/{dto.Id}",
                ContentType = SD.ContentType.MultipartFormData
            });
        }
    }
}