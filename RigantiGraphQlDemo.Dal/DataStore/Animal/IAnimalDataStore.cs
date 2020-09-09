using System;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Dal.DataStore.Animal
{
    public interface IAnimalDataStore
    {
        IObservable<Entities.Animal> WhenAnimalCreated { get; }

        Task<Entities.Animal> CreateAnimalAsync(Entities.Animal animal);
        Task<Entities.Animal> UpdateAnimalAsync(int animalId, Entities.Animal animal);
        Task<Entities.Animal> DeleteAnimalAsync(int animalId);
    }
}