using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RigantiGraphQlDemo.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Farm
{
    public class FarmsByPersonIdDataLoader : GroupedDataLoader<int, Dal.Entities.Farm>
    {
        private readonly DbContextPool<AnimalFarmDbContext> dbContextPool;

        public FarmsByPersonIdDataLoader(
            DbContextPool<AnimalFarmDbContext> dbContextPool,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<int>? options = null)
            : base(batchScheduler, options)
        {
            this.dbContextPool = dbContextPool ?? throw new ArgumentNullException(nameof(dbContextPool));
        }

        protected override async Task<ILookup<int, Dal.Entities.Farm>> LoadGroupedBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            AnimalFarmDbContext dbContext = dbContextPool.Rent();
            try
            {
                var farms = await dbContext.Farms.Where(f => keys.Contains(f.PersonId)).ToListAsync(cancellationToken);
                return farms.ToLookup(t => t.PersonId);
            }
            finally
            {
                dbContextPool.Return(dbContext);
            }
        }
    }
}