using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Dal.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person
                {
                    Id = 1,
                    Name = "Mr. Jones",
                    SecretPiggyBankLocation = "In a dark cave."
                },
                new Person
                {
                    Id = 2,
                    Name = "Mr. Whymper",
                    SecretPiggyBankLocation = "Does not have a piggy bank."
                }
            );

            modelBuilder.Entity<Farm>().HasData(
                new Farm
                {
                    Id = 1,
                    Name = "Manor Farm",
                    PersonId = 1
                },
                new Farm
                {
                    Id = 2,
                    Name = "AnimalFarm",
                    PersonId = 2
                }
            );

            modelBuilder.Entity<Animal>().HasData(
                new Animal
                {
                    Id = 1,
                    Name = "Napoleon",
                    Species = "Pig",
                    FarmId = 1
                },
                new Animal
                {
                    Id = 2,
                    Name = "Snowball",
                    Species = "Pig",
                    FarmId = 1
                },
                new Animal
                {
                    Id = 3,
                    Name = "Boxer",
                    Species = "Horse",
                    FarmId = 1
                },
                new Animal
                {
                    Id = 4,
                    Name = "Moses",
                    Species = "Raven",
                    FarmId = 1
                },
                new Animal
                {
                    Id = 5,
                    Name = "Benjamin",
                    Species = "Donkey",
                    FarmId = 1
                },
                new Animal
                {
                    Id = 6,
                    Name = "AnonymousCat",
                    Species = "Cat",
                    FarmId = 2
                },
                new Animal
                {
                    Id = 7,
                    Name = "AnonymousGoat",
                    Species = "Goat",
                    FarmId = 2
                }
            );
        }
    }
}