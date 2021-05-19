using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
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
        private readonly IDbContextFactory<AnimalFarmDbContext> dbContextFactory;

        public AnimalByFarmIdDataLoader(
            IDbContextFactory<AnimalFarmDbContext> dbContextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<int>? options = null)
            : base(batchScheduler, options)
        {
            this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<ILookup<int, Dal.Entities.Animal>> LoadGroupedBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using var dbContext = dbContextFactory.CreateDbContext();
         
            var animals = await dbContext.Animals.Where(f => keys.Contains(f.FarmId)).ToListAsync(cancellationToken);
            return animals.ToLookup(t => t.FarmId);
        }
    }
}