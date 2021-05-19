using GreenDonut;
using Microsoft.EntityFrameworkCore.Internal;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Common;
using RigantiGraphQlDemo.Dal;

namespace RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Animal
{
    public class AnimalByIdDataLoader : EntityByIdDataLoaderBase<Dal.Entities.Animal>
    {
        public AnimalByIdDataLoader(
            DbContextPool<AnimalFarmDbContext> dbContextPool,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<int>? options = null)
            : base(dbContextPool, batchScheduler, options)
        {
        }
    }
}