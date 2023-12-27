using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(Villa entity)
        {
            await _db.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _db.Villas;

            query = !tracked ? query.AsNoTracking() : query;

            query = filter != null ? query.Where(filter) : query;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null)
        {
            IQueryable<Villa> query = _db.Villas;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();

        }

        public async Task RemoveAsync(Villa entity)
        {
            _db.Villas.Remove(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync(Villa entity)
        {
            _db.Villas.Update(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}