using RigantiGraphQlDemo.Dal.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Dal.DataStore
{
    public interface IDataStore
    {
        Task<IEnumerable<Person>> GetPersonsAsync();
        Task<IEnumerable<Farm>> GetFarmsAsync();
        Task<IEnumerable<Farm>> GetFarmsByPersonIdAsync(int personId);
        Task<Animal> CreateAnimalAsync(Animal animal);
        Task<Animal> UpdateAnimalAsync(int animalId, Animal animal);
        Task<Animal> DeleteAnimalAsync(int animalId);
        Task<Person> GetPersonByIdAsync(int personId);
        Task<ILookup<int, Farm>> GetFarmsByPersonIdDataLoaderAsync(IEnumerable<int> personIds, CancellationToken token);
    }
}