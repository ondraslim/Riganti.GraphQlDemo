using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Dal
{
    public class AnimalFarmDbContext : DbContext
    {
        private readonly ILoggerFactory loggerFactory;

        public AnimalFarmDbContext(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<Animal> Animals { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=animalFarm.db");
            options.UseLoggerFactory(loggerFactory);
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
        }
    }
}