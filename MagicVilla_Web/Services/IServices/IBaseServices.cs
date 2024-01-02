using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_Web.Models;

namespace MagicVilla_Web.Services.IServices
{
    public interface IBaseServices
    {
        APIResponse responseModel {get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}