using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
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
        private readonly IDbContextFactory<AnimalFarmDbContext> dbContextFactory;

        public FarmsByPersonIdDataLoader(
            IDbContextFactory<AnimalFarmDbContext> dbContextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<int>? options = null)
            : base(batchScheduler, options)
        {
            this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<ILookup<int, Dal.Entities.Farm>> LoadGroupedBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using var dbContext = dbContextFactory.CreateDbContext();

            var farms = await dbContext.Farms.Where(f => keys.Contains(f.PersonId)).ToListAsync(cancellationToken);
            return farms.ToLookup(t => t.PersonId);
        }
    }
}