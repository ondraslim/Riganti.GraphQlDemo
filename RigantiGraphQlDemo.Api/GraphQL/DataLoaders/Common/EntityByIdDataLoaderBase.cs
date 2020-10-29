using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
        private readonly DbContextPool<AnimalFarmDbContext> dbContextPool;

        public EntityByIdDataLoaderBase(DbContextPool<AnimalFarmDbContext> dbContextPool)
        {
            this.dbContextPool = dbContextPool ?? throw new ArgumentNullException(nameof(dbContextPool));
        }

        protected override async Task<IReadOnlyDictionary<int, TEntity>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            AnimalFarmDbContext dbContext = dbContextPool.Rent();
            try
            {
                return await dbContext.Set<TEntity>()
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