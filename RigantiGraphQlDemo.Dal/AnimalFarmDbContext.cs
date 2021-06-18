using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Dal.Entities;
using RigantiGraphQlDemo.Dal.Extensions;

namespace RigantiGraphQlDemo.Dal
{
    public class AnimalFarmDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; } = default!;
        public DbSet<Farm> Farms { get; set; } = default!;
        public DbSet<Animal> Animals { get; set; } = default!;

        public AnimalFarmDbContext(DbContextOptions<AnimalFarmDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasMany(t => t.Farms)
                .WithOne(t => t.Person)
                .HasForeignKey(t => t.PersonId);


            modelBuilder.Entity<Farm>()
                .HasMany(t => t.Animals)
                .WithOne(t => t.Farm)
                .HasForeignKey(t => t.FarmId);


           modelBuilder.Seed();
        }
    }
}