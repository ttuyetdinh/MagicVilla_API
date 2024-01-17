using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_Ultility;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void ClearToken()
        {
            _contextAccessor.HttpContext.Response.Cookies.Delete(SD.AccessToken);
        }

        public TokenDTO? GetToken()
        {
            try
            {
                bool hasAccessToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.AccessToken, out string? accessToken);
                var tokenDto = new TokenDTO()
                {
                    AccessToken = accessToken
                };

                return hasAccessToken ? tokenDto : null;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public void SetToken(TokenDTO tokenDTO)
        {
            var cookiesOptions = new CookieOptions() { Expires = DateTime.Now.AddDays(30) };
            _contextAccessor.HttpContext.Response.Cookies.Append(SD.AccessToken, tokenDTO.AccessToken, cookiesOptions);
        }
    }
}