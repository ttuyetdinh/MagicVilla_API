using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MagicVilla_Ultility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using static MagicVilla_Ultility.SD;

namespace MagicVilla_Web.Services
{
    public class BaseServices : IBaseServices
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseServices(IHttpClientFactory httpClient)
        {
            responseModel = new APIResponse();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");

                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }
                message.Method = GetHttpMethod(apiRequest.ApiType);

                HttpResponseMessage apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                try
                {
                    APIResponse response = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if( response.StatusCode == System.Net.HttpStatusCode.NotFound 
                        || response.StatusCode == System.Net.HttpStatusCode.BadRequest){
                            response.IsSuccess = false;
                    }
                    var returnObj = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(response));

                    return returnObj;
                }
                catch (Exception e)
                {

                    var response = JsonConvert.DeserializeObject<T>(apiContent);
                    return response;
                }


            }
            catch (Exception e)
            {
                var dto = new APIResponse
                {
                    ErrorMessage = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var response = JsonConvert.DeserializeObject<T>(res);

                return response;
            }
        }


        // ultility methods
        private HttpMethod GetHttpMethod(ApiType apiType)
        {
            return apiType switch
            {
                ApiType.GET => HttpMethod.Get,
                ApiType.POST => HttpMethod.Post,
                ApiType.PUT => HttpMethod.Put,
                ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get,
            };
        }
    }
}