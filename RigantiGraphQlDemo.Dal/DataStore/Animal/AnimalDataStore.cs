using Microsoft.EntityFrameworkCore;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Dal.DataStore.Animal
{
    public class AnimalDataStore : IAnimalDataStore, IDisposable
    {
        private readonly AnimalFarmDbContext dbContext; 
        

        private readonly Subject<Entities.Animal> animalCreated;
        public IObservable<Entities.Animal> AnimalCreated => animalCreated.AsObservable();


        public AnimalDataStore(AnimalFarmDbContext dbContext)
        {
            animalCreated = new Subject<Entities.Animal>();
            this.dbContext = dbContext;
        }


        #region Mutate
        public async Task<Entities.Animal> CreateAnimalAsync(Entities.Animal animal)
        {
            var addedAnimal = await dbContext.Animals.AddAsync(animal);
            await dbContext.SaveChangesAsync();

            animalCreated.OnNext(animal);
            return addedAnimal.Entity;
        }
        public async Task<Entities.Animal> UpdateAnimalAsync(int animalId, Entities.Animal animal)
        {
            var storedAnimal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId);
            if (storedAnimal == default) return null;

            storedAnimal.Name = animal.Name;
            storedAnimal.Species = animal.Species;
            storedAnimal.FarmId = animal.FarmId;

            await dbContext.SaveChangesAsync();

            return storedAnimal;
        }
        public async Task<Entities.Animal> DeleteAnimalAsync(int animalId)
        {
            var storedAnimal = await dbContext.Animals.FirstOrDefaultAsync(a => a.Id == animalId);
            if (storedAnimal == default) return null;

            dbContext.Animals.Remove(storedAnimal);
            await dbContext.SaveChangesAsync();

            return storedAnimal;
        }

        #endregion

        public void Dispose()
        {
            dbContext?.Dispose();
            animalCreated?.Dispose();
        }
    }
}