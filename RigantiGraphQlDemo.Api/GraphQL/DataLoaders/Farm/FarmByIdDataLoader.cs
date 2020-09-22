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
    public class FarmByIdDataLoader : BatchDataLoader<int, Dal.Entities.Farm>        // TODO: common generic BatchDataLoader
    {
        private readonly DbContextPool<AnimalFarmDbContext> dbContextPool;

        public FarmByIdDataLoader(DbContextPool<AnimalFarmDbContext> dbContextPool)
        {
            this.dbContextPool = dbContextPool ?? throw new ArgumentNullException(nameof(dbContextPool));
        }

        protected override async Task<IReadOnlyDictionary<int, Dal.Entities.Farm>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            AnimalFarmDbContext dbContext = dbContextPool.Rent();
            try
            {
                return await dbContext.Farms
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