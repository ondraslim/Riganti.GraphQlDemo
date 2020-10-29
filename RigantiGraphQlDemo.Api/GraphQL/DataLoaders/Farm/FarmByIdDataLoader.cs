using Microsoft.EntityFrameworkCore.Internal;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Common;
using RigantiGraphQlDemo.Dal;

namespace RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Farm
{
    public class FarmByIdDataLoader : EntityByIdDataLoaderBase<Dal.Entities.Farm>
    {
        public FarmByIdDataLoader(DbContextPool<AnimalFarmDbContext> dbContextPool)
            : base(dbContextPool)
        {
        }
    }
}