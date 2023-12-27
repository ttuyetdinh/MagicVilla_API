using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MagicVilla_VillaAPI.Model;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task<List<Villa>> GetAllAsync (Expression<Func<Villa, bool >> filter = null);
        Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true);
        Task CreateAsync(Villa entity);
        Task RemoveAsync(Villa entity);
        Task UpdateAsync(Villa entity);
        Task SaveAsync();

    }
}