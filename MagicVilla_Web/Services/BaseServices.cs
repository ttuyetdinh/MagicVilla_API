using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

        private readonly ITokenProvider _tokenProvider;
        public BaseServices(IHttpClientFactory httpClient, ITokenProvider tokenProvider)
        {
            responseModel = new APIResponse();
            this.httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest, bool withBearer = true)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");

                HttpRequestMessage message = new HttpRequestMessage
                {
                    RequestUri = new Uri(apiRequest.Url)
                };

                if (apiRequest.ContentType == ContentType.MultipartFormData)
                {
                    message.Headers.Add("Accept", "*/*");
                    var content = new MultipartFormDataContent();
                    foreach (var prop in apiRequest.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(apiRequest.Data);
                        if (value is FormFile)
                        {
                            var file = (FormFile)value;
                            if (file != null) content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                        }
                        else
                        {
                            content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                        }
                    }
                    message.Content = content;
                }
                else
                {
                    message.Headers.Add("Accept", "application/json");

                    if (apiRequest.Data != null)
                    {
                        message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                    }
                }
                message.Method = GetHttpMethod(apiRequest.ApiType);

                // get JWT token from the cookie
                if (withBearer == true && _tokenProvider.GetToken() != null)
                {
                    var token = _tokenProvider.GetToken();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                }

                HttpResponseMessage apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                try
                {
                    APIResponse response = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (response != null && (response.StatusCode == System.Net.HttpStatusCode.NotFound
                        || response.StatusCode == System.Net.HttpStatusCode.BadRequest))
                    {
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