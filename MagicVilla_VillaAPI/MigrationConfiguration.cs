using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI
{
    public static class MigrationConfiguration
    {
        public static void ApplyMigration(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
        }
    }
}