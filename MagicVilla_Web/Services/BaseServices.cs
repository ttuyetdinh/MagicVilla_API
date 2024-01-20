using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MagicVilla_Ultility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using static MagicVilla_Ultility.SD;

namespace MagicVilla_Web.Services
{
    public class BaseServices : IBaseServices
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        private readonly ITokenProvider _tokenProvider;
        private readonly string VillaApiUrl;
        private IHttpContextAccessor _httpContextAccessor;
        private IApiMessageRequestBuilder _apiMessageRequestBuilder;
        public BaseServices(IHttpClientFactory httpClient, ITokenProvider tokenProvider,
                            IConfiguration configuration, IHttpContextAccessor httpContextAccessor,
                            IApiMessageRequestBuilder apiMessageRequestBuilder)
        {
            responseModel = new APIResponse();
            this.httpClient = httpClient;
            _tokenProvider = tokenProvider;
            VillaApiUrl = configuration.GetValue<string>("ServiceUrls:VillaUrl");
            _httpContextAccessor = httpContextAccessor;
            _apiMessageRequestBuilder = apiMessageRequestBuilder;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest, bool withBearer = true)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");

                // create a new HttpRequestMessage instance every time messageFactory() is called
                // to avoid the InvalidOperationException because HttpRequestMessage cannot be reused after it's been sent.
                var messageFactory = () =>
                {
                    return _apiMessageRequestBuilder.Build(apiRequest);
                };

                // call function to hanlde the send with refresh token
                HttpResponseMessage httpResponseMessage = await SendWithRefreshTokenAsync(client, messageFactory, withBearer);
                APIResponse finalApiResponse = new()
                {
                    IsSuccess = false
                };
                try
                {
                    switch (httpResponseMessage.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            finalApiResponse.ErrorMessage = new List<string>() { "Not found" };
                            break;
                        case HttpStatusCode.Forbidden:
                            finalApiResponse.ErrorMessage = new List<string>() { "Access denied" };
                            break;
                        case HttpStatusCode.Unauthorized:
                            finalApiResponse.ErrorMessage = new List<string>() { "Unauthorized" };
                            break;
                        case HttpStatusCode.InternalServerError:
                            finalApiResponse.ErrorMessage = new List<string>() { "Internal server error" };
                            break;
                        default:
                            var apiContent = await httpResponseMessage.Content.ReadAsStringAsync();
                            finalApiResponse.IsSuccess = true;
                            finalApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                            break;
                    }

                }

                catch (Exception e)
                {
                    finalApiResponse.ErrorMessage = new List<string>() { "Error countered:", e.Message.ToString() };
                }

                var returnObj = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(finalApiResponse));
                return returnObj;
            }
            // a spesific exception must be above all exception
            catch (AuthException ae)
            {
                throw;
            }
            catch (Exception e)
            {
                var dto = new APIResponse
                {
                    ErrorMessage = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };

                var errorResponse = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dto));

                return errorResponse;
            }
        }


        // ultility methods

        private async Task<HttpResponseMessage> SendWithRefreshTokenAsync(
            HttpClient httpClient,
            Func<HttpRequestMessage> httpRequestMessageFactory,
            bool withBearer = true)
        {
            if (!withBearer) return await httpClient.SendAsync(httpRequestMessageFactory());

            // add token to header before sending request
            var tokenDTO = _tokenProvider.GetToken();
            if (tokenDTO != null && !string.IsNullOrEmpty(tokenDTO.AccessToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenDTO.AccessToken);
            }

            try
            {
                var response = await httpClient.SendAsync(httpRequestMessageFactory());
                if (response.IsSuccessStatusCode) return response;

                // unauthorized mean access token is expired
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await InvokeRefreshTokenEndPoint(httpClient, tokenDTO);
                    response = await httpClient.SendAsync(httpRequestMessageFactory());
                    return response;
                }
                return response;
            }
            catch (AuthException ae)
            {
                throw;
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // refresh token and retry request
                    await InvokeRefreshTokenEndPoint(httpClient, tokenDTO);
                    return await httpClient.SendAsync(httpRequestMessageFactory());
                }
                throw;
            }
        }

        private async Task InvokeRefreshTokenEndPoint(HttpClient httpClient, TokenDTO tokenDTO)
        {
            var message = new HttpRequestMessage();

            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri($"{VillaApiUrl}/api/{SD.CurrentAPIVersion}/UsersAuth/refresh");
            message.Method = HttpMethod.Post;
            message.Content = new StringContent(
                JsonConvert.SerializeObject(
                    new TokenDTO()
                    {
                        AccessToken = tokenDTO.AccessToken,
                        RefreshToken = tokenDTO.RefreshToken
                    }
                ),
                Encoding.UTF8, "application/json"
            );

            var response = await httpClient.SendAsync(message);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<APIResponse>(content);

            // handle weired case when api response is not valid
            if (apiResponse?.IsSuccess != true)
            {
                await _httpContextAccessor.HttpContext.SignOutAsync();
                _tokenProvider.ClearToken();
                throw new AuthException();
            }
            else
            {
                // extract new token and sign in again with new access token
                var newTokenDTO = JsonConvert.DeserializeObject<TokenDTO>(apiResponse.Result.ToString());

                if (tokenDTO != null && !string.IsNullOrEmpty(tokenDTO.AccessToken))
                {
                    await SignInWithNewToken(newTokenDTO);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newTokenDTO.AccessToken);
                }
            }
        }

        private async Task SignInWithNewToken(TokenDTO tokenDTO)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(tokenDTO.AccessToken);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            _tokenProvider.SetToken(tokenDTO);

        }
    }
}