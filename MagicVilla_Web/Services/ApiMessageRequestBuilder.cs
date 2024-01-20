using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using static MagicVilla_Ultility.SD;

namespace MagicVilla_Web.Services
{
    public class ApiMessageRequestBuilder : IApiMessageRequestBuilder
    {
        public HttpRequestMessage Build(APIRequest apiRequest)
        {
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

            return message;
        }

        // ultilities function
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