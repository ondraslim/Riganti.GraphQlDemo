using GreenDonut;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Common;
using RigantiGraphQlDemo.Dal;

namespace RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Farm
{
    public class FarmByIdDataLoader : EntityByIdDataLoaderBase<Dal.Entities.Farm>
    {
        public FarmByIdDataLoader(
            IDbContextFactory<AnimalFarmDbContext> dbContextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<int>? options = null)
            : base(dbContextFactory, batchScheduler, options)
        {
        }
    }
}