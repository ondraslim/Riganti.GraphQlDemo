using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Common
{
    public class EntityByIdDataLoaderBase<TEntity> : BatchDataLoader<int, TEntity>
    where TEntity : EntityBase
    {
        private readonly IDbContextFactory<AnimalFarmDbContext> dbContextFactory;

        public EntityByIdDataLoaderBase(
            IDbContextFactory<AnimalFarmDbContext> dbContextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<int>? options = null)
            : base(batchScheduler, options)
        {
            this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<int, TEntity>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using var dbContext = dbContextFactory.CreateDbContext();

            return await dbContext.Set<TEntity>()
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}