using Microsoft.EntityFrameworkCore.Internal;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Common;
using RigantiGraphQlDemo.Dal;

namespace RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Person
{
    public class PersonByIdDataLoader : EntityByIdDataLoaderBase<Dal.Entities.Person>
    {
        public PersonByIdDataLoader(DbContextPool<AnimalFarmDbContext> dbContextPool)
            : base(dbContextPool)
        {
        }
    }
}