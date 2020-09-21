using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.DataLoaders
{
    public class FarmsByPersonIdDataLoader : GroupedDataLoader<int, Farm>
    {
        private readonly DbContextPool<AnimalFarmDbContext> dbContextPool;

        public FarmsByPersonIdDataLoader(DbContextPool<AnimalFarmDbContext> dbContextPool)
        {
            this.dbContextPool = dbContextPool ?? throw new ArgumentNullException(nameof(dbContextPool));
        }

        protected override async Task<ILookup<int, Farm>> LoadGroupedBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            AnimalFarmDbContext dbContext = dbContextPool.Rent();
            try
            {
                var farms = await dbContext.Farms.Where(f => keys.Contains(f.PersonId)).ToListAsync(cancellationToken);
                return farms.ToLookup(t => t.Id);
            }
            finally
            {
                dbContextPool.Return(dbContext);
            }
        }
    }
}