using RigantiGraphQlDemo.Api.GraphQL.DataLoaders;
using RigantiGraphQlDemo.Dal.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.Resolvers
{
    public class AnimalResolvers
    {
        public async Task<IEnumerable<Animal>> GetAnimalsByFarmIdsAsync(
            Farm farm,
            AnimalByFarmIdDataLoader dataLoader,
            CancellationToken token) =>
            await dataLoader.LoadAsync(farm.Id, token);
    }
}