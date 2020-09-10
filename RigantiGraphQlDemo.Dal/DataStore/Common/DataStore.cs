using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Dal.DataStore.Common
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
            return await dbContext.Farms.Include(f => f.Animals).AsNoTracking().ToListAsync();
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