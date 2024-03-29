using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_VillaAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MagicVilla_VillaAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        

        // add sameple data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VillaNumber>().HasKey(c=> new{c.Id});

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
                    Age = 23
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
                    Age = 23
                },
                new Villa
                {
                    Id = 3,
                    Name = "Luxury Pool Villa",
                    Details = "Hghhhhh",
                    ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa4.jpg",
                    Occupancy = 4,
                    Rate = 400,
                    Sqft = 750,
                    Amenity = "",
                    Age = 23
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
                    Age = 23
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
                    Age = 23
                }
            );

            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber
                {   Id = 1,
                    VillaRoom = 100,
                    SpecialDetails = "detail of first num",
                    VillaId = 1

                },
                new VillaNumber
                {   
                    Id = 2,
                    VillaRoom = 200,
                    SpecialDetails = "detail of second num",
                    VillaId = 2

                },
                new VillaNumber
                {   
                    Id = 3,
                    VillaRoom = 300,
                    SpecialDetails = "detail of third num",
                    VillaId = 2

                }
            );
        }
    }
}