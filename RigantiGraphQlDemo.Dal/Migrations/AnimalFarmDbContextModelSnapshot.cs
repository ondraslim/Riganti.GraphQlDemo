﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RigantiGraphQlDemo.Dal;

namespace RigantiGraphQlDemo.Dal.Migrations
{
    [DbContext(typeof(AnimalFarmDbContext))]
    partial class AnimalFarmDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7");

            modelBuilder.Entity("RigantiGraphQlDemo.Dal.Entities.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FarmId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Species")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FarmId");

                    b.ToTable("Animals");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FarmId = 1,
                            Name = "Napoleon",
                            Species = "Pig"
                        },
                        new
                        {
                            Id = 2,
                            FarmId = 1,
                            Name = "Snowball",
                            Species = "Pig"
                        },
                        new
                        {
                            Id = 3,
                            FarmId = 1,
                            Name = "Boxer",
                            Species = "Horse"
                        },
                        new
                        {
                            Id = 4,
                            FarmId = 1,
                            Name = "Moses",
                            Species = "Raven"
                        },
                        new
                        {
                            Id = 5,
                            FarmId = 1,
                            Name = "Benjamin",
                            Species = "Donkey"
                        },
                        new
                        {
                            Id = 6,
                            FarmId = 2,
                            Name = "AnonymousCat",
                            Species = "Cat"
                        },
                        new
                        {
                            Id = 7,
                            FarmId = 2,
                            Name = "AnonymousGoat",
                            Species = "Goat"
                        });
                });

            modelBuilder.Entity("RigantiGraphQlDemo.Dal.Entities.Farm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("PersonId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Farms");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Manor Farm",
                            PersonId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "AnimalFarm",
                            PersonId = 2
                        });
                });

            modelBuilder.Entity("RigantiGraphQlDemo.Dal.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("SecretPiggyBankLocation")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Persons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Mr. Jones",
                            SecretPiggyBankLocation = "In a dark cave."
                        },
                        new
                        {
                            Id = 2,
                            Name = "Mr. Whymper",
                            SecretPiggyBankLocation = "Does not have a piggy bank."
                        });
                });

            modelBuilder.Entity("RigantiGraphQlDemo.Dal.Entities.Animal", b =>
                {
                    b.HasOne("RigantiGraphQlDemo.Dal.Entities.Farm", "Farm")
                        .WithMany("Animals")
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RigantiGraphQlDemo.Dal.Entities.Farm", b =>
                {
                    b.HasOne("RigantiGraphQlDemo.Dal.Entities.Person", "Person")
                        .WithMany("Farms")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
