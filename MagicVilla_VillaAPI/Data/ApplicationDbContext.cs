using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_VillaAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "BaoThw",
                    ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa3.jpg",
                    Occupancy = 4,
                    Rate = 200,
                    Sqft = 550,
                    Amenity = "",
                    Age = 23,
                    CreatedDate = DateTime.Now
                },
                new Villa
                {
                    Id = 2,
                    Name = "Premium Pool Villa",
                    Details = "Baotrxn",
                    ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa1.jpg",
                    Occupancy = 4,
                    Rate = 300,
                    Sqft = 550,
                    Amenity = "",
                    Age = 23,
                    CreatedDate = DateTime.Now
                },
                new Villa
                {
                    Id = 3,
                    Name = "Luxury Pool Villa",
                    Details = "Hgh",
                    ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa4.jpg",
                    Occupancy = 4,
                    Rate = 400,
                    Sqft = 750,
                    Amenity = "",
                    Age = 23,
                    CreatedDate = DateTime.Now
                },
                new Villa
                {
                    Id = 4,
                    Name = "Diamond Villa",
                    Details = "Lanvieee",
                    ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa5.jpg",
                    Occupancy = 4,
                    Rate = 550,
                    Sqft = 900,
                    Amenity = "",
                    Age = 23,
                    CreatedDate = DateTime.Now
                },
                new Villa
                {
                    Id = 5,
                    Name = "Diamond Pool Villa",
                    Details = "yenle",
                    ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa2.jpg",
                    Occupancy = 4,
                    Rate = 600,
                    Sqft = 1100,
                    Amenity = "",
                    Age = 23,
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}