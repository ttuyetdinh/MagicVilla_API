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
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {   
            entity.UpdatedDate = DateTime.Now;
            _db.VillaNumbers.Update(entity);
            
            // can use the method inherit from the Repository<Villa>
            // because they are reference to the same dbcontext instance
            // await _db.SaveChangesAsync();
            await SaveAsync();

            return entity;
        }
    }
}