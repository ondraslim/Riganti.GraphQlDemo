using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Dal.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Dal.DataStore
{
    public class DataStore : IDataStore
    {
        private readonly AnimalFarmDbContext dbContext;

        public DataStore(AnimalFarmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region GET
        public async Task<IEnumerable<Person>> GetPersonsAsync()
        {
            return await dbContext.Persons.AsNoTracking().ToListAsync();
        }

        public Task<Person> GetPersonByIdAsync(int personId)
        {
            return dbContext.Persons.AsNoTracking().FirstOrDefaultAsync(p => p.Id == personId);
        }

        public async Task<IEnumerable<Farm>> GetFarmsAsync()
        {
            return await dbContext.Farms.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Farm>> GetFarmsByPersonIdAsync(int personId)
        {
            return await dbContext.Farms.Where(f => personId == f.PersonId).ToListAsync();
        }

        #endregion

        #region Mutate
        public async Task<Animal> CreateAnimalAsync(Animal animal)
        {
            var addedAnimal = await dbContext.Animals.AddAsync(animal);
            await dbContext.SaveChangesAsync();

            return addedAnimal.Entity;
        }
        public async Task<Animal> UpdateAnimalAsync(int animalId, Animal animal)
        {
            var storedAnimal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId);
            if (storedAnimal == default) return null;

            storedAnimal.Name = animal.Name;
            storedAnimal.Species = animal.Species;
            storedAnimal.FarmId = animal.FarmId;

            await dbContext.SaveChangesAsync();

            return storedAnimal;
        }
        public async Task<Animal> DeleteAnimalAsync(int animalId)
        {
            var storedAnimal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId);
            if (storedAnimal == default) return null;

            dbContext.Animals.Remove(storedAnimal);
            await dbContext.SaveChangesAsync();

            return storedAnimal;
        }

        #endregion


        #region DataLoaders

        public async Task<ILookup<int, Farm>> GetFarmsByPersonIdDataLoaderAsync(IEnumerable<int> personIds, CancellationToken token)
        {
            var farms = await dbContext.Farms.Where(f => personIds.Contains(f.PersonId)).ToListAsync(token);
            return farms.ToLookup(f => f.PersonId);
        }

        public async Task<IDictionary<int, Person>> GetPersonsByIdDataLoaderAsync(IEnumerable<int> personIds, CancellationToken token)
        {
            return await dbContext.Persons.Where(p => personIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id, token);
        }

        #endregion
    }
}