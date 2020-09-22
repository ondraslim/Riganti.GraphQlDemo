using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RigantiGraphQlDemo.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Person
{
    public class PersonByIdDataLoader : BatchDataLoader<int, Dal.Entities.Person>        // TODO: common generic BatchDataLoader
    {
        private readonly DbContextPool<AnimalFarmDbContext> dbContextPool;

        public PersonByIdDataLoader(DbContextPool<AnimalFarmDbContext> dbContextPool)
        {
            this.dbContextPool = dbContextPool ?? throw new ArgumentNullException(nameof(dbContextPool));
        }

        protected override async Task<IReadOnlyDictionary<int, Dal.Entities.Person>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            AnimalFarmDbContext dbContext = dbContextPool.Rent();
            try
            {
                return await dbContext.Persons
                    .Where(s => keys.Contains(s.Id))
                    .ToDictionaryAsync(t => t.Id, cancellationToken);
            }
            finally
            {
                dbContextPool.Return(dbContext);
            }
        }
    }
}