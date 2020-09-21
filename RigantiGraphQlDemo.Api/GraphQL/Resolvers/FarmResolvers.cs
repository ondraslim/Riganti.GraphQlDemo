using RigantiGraphQlDemo.Api.GraphQL.DataLoaders;
using RigantiGraphQlDemo.Dal.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.Resolvers
{
    public class FarmResolvers
    {
        public async Task<IEnumerable<Farm>> GetFarmsByPersonIdsAsync(
            Person person,
            FarmsByPersonIdDataLoader dataLoader,
            CancellationToken token) =>
            await dataLoader.LoadAsync(person.Id, token);
    }
}