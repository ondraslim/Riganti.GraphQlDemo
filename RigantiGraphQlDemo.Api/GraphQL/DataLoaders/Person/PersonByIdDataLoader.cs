using GreenDonut;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Common;
using RigantiGraphQlDemo.Dal;

namespace RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Person
{
    public class PersonByIdDataLoader : EntityByIdDataLoaderBase<Dal.Entities.Person>
    {
        public PersonByIdDataLoader(
            IDbContextFactory<AnimalFarmDbContext> dbContextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<int>? options = null)
            : base(dbContextFactory, batchScheduler, options)
        {
        }
    }
}