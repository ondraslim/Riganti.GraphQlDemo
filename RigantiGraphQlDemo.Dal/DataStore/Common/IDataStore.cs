using RigantiGraphQlDemo.Dal.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Dal.DataStore.Common
{
    public interface IDataStore
    {
        Task<IEnumerable<Person>> GetPersonsAsync();
        Task<IEnumerable<Farm>> GetFarmsAsync();
        Task<Person> GetPersonByIdAsync(int personId);
        Task<ILookup<int, Farm>> GetFarmsByPersonIdDataLoaderAsync(IEnumerable<int> personIds, CancellationToken token);
        Task<IDictionary<int, Person>> GetPersonsByIdDataLoaderAsync(IEnumerable<int> personIds, CancellationToken token);
    }
}