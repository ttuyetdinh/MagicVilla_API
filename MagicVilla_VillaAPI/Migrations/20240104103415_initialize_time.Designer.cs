﻿// <auto-generated />
using System;
using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240104103415_initialize_time")]
    partial class initialize_time
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MagicVilla_VillaAPI.Model.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Amenity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Occupancy")
                        .HasColumnType("int");

                    b.Property<double?>("Rate")
                        .HasColumnType("float");

                    b.Property<int?>("Sqft")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Age = 23,
                            Amenity = "",
                            CreatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5891),
                            Details = "BaoThw",
                            ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa3.jpg",
                            Name = "Royal Villa",
                            Occupancy = 4,
                            Rate = 200.0,
                            Sqft = 550,
                            UpdatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5900)
                        },
                        new
                        {
                            Id = 2,
                            Age = 23,
                            Amenity = "",
                            CreatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5903),
                            Details = "Baotrxn",
                            ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa1.jpg",
                            Name = "Premium Pool Villa",
                            Occupancy = 4,
                            Rate = 300.0,
                            Sqft = 550,
                            UpdatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5904)
                        },
                        new
                        {
                            Id = 3,
                            Age = 23,
                            Amenity = "",
                            CreatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5906),
                            Details = "Hgh",
                            ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa4.jpg",
                            Name = "Luxury Pool Villa",
                            Occupancy = 4,
                            Rate = 400.0,
                            Sqft = 750,
                            UpdatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5906)
                        },
                        new
                        {
                            Id = 4,
                            Age = 23,
                            Amenity = "",
                            CreatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5908),
                            Details = "Lanvieee",
                            ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa5.jpg",
                            Name = "Diamond Villa",
                            Occupancy = 4,
                            Rate = 550.0,
                            Sqft = 900,
                            UpdatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5908)
                        },
                        new
                        {
                            Id = 5,
                            Age = 23,
                            Amenity = "",
                            CreatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5910),
                            Details = "yenle",
                            ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa2.jpg",
                            Name = "Diamond Pool Villa",
                            Occupancy = 4,
                            Rate = 600.0,
                            Sqft = 1100,
                            UpdatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5911)
                        });
                });

            modelBuilder.Entity("MagicVilla_VillaAPI.Model.VillaNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SpecialDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("VillaId")
                        .HasColumnType("int");

                    b.Property<int>("VillaRoom")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VillaId");

                    b.ToTable("VillaNumbers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(6050),
                            SpecialDetails = "detail of first num",
                            UpdatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(6050),
                            VillaId = 1,
                            VillaRoom = 100
                        },
                        new
                        {
                            Id = 2,
                            CreatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(6052),
                            SpecialDetails = "detail of second num",
                            UpdatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(6053),
                            VillaId = 2,
                            VillaRoom = 200
                        },
                        new
                        {
                            Id = 3,
                            CreatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(6054),
                            SpecialDetails = "detail of third num",
                            UpdatedDate = new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(6054),
                            VillaId = 2,
                            VillaRoom = 300
                        });
                });

            modelBuilder.Entity("MagicVilla_VillaAPI.Model.VillaNumber", b =>
                {
                    b.HasOne("MagicVilla_VillaAPI.Model.Villa", "Villa")
                        .WithMany("VillaNumbers")
                        .HasForeignKey("VillaId");

                    b.Navigation("Villa");
                });

            modelBuilder.Entity("MagicVilla_VillaAPI.Model.Villa", b =>
                {
                    b.Navigation("VillaNumbers");
                });
#pragma warning restore 612, 618
        }
    }
}
