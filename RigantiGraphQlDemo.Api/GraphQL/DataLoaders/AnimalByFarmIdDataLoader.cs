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
    public class AnimalByFarmIdDataLoader : GroupedDataLoader<int, Animal>
    {
        private readonly DbContextPool<AnimalFarmDbContext> dbContextPool;

        public AnimalByFarmIdDataLoader(DbContextPool<AnimalFarmDbContext> dbContextPool)
        {
            this.dbContextPool = dbContextPool ?? throw new ArgumentNullException(nameof(dbContextPool));
        }

        protected override async Task<ILookup<int, Animal>> LoadGroupedBatchAsync(
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