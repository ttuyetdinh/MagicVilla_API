using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }
        public virtual async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            query = !tracked ? query.AsNoTracking() : query;

            query = filter != null ? query.Where(filter) : query;

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                };
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, Pagination? pagination = null)
        {
            IQueryable<T> query = dbSet;
            int pageSize = pagination?.PageSize ?? 0;
            int pageNum = pagination?.PageNum ?? 0;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (pageSize > 0 && pageNum > 0)
            {
                pageSize = pageSize > 100 ? 100 : pageSize;
                query = query.Skip(pageSize * (pageNum - 1)).Take(pageSize);
            }

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                };
            }

            return await query.ToListAsync();

        }

        public virtual async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public virtual async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}