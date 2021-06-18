using GreenDonut;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Common;
using RigantiGraphQlDemo.Dal;

namespace RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Animal
{
    public class AnimalByIdDataLoader : EntityByIdDataLoaderBase<Dal.Entities.Animal>
    {
        public AnimalByIdDataLoader(
            IDbContextFactory<AnimalFarmDbContext> dbContextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<int>? options = null)
            : base(dbContextFactory, batchScheduler, options)
        {
        }
    }
}