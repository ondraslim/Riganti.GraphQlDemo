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

namespace RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Animal
{
    public class AnimalByFarmIdDataLoader : GroupedDataLoader<int, Dal.Entities.Animal>
    {
        private readonly DbContextPool<AnimalFarmDbContext> dbContextPool;

        public AnimalByFarmIdDataLoader(
            DbContextPool<AnimalFarmDbContext> dbContextPool,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<int>? options = null)
            : base(batchScheduler, options)
        {
            this.dbContextPool = dbContextPool ?? throw new ArgumentNullException(nameof(dbContextPool));
        }

        protected override async Task<ILookup<int, Dal.Entities.Animal>> LoadGroupedBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            AnimalFarmDbContext dbContext = dbContextPool.Rent();
            try
            {
                var animals = await dbContext.Animals.Where(f => keys.Contains(f.FarmId)).ToListAsync(cancellationToken);
                return animals.ToLookup(t => t.FarmId);
            }
            finally
            {
                dbContextPool.Return(dbContext);
            }
        }
    }
}