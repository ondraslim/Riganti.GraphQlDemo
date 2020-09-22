using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Farm;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.Query
{
    [ExtendObjectType(Name = "Query")]
    public class FarmQueries
    {
        [UseApplicationDbContext]
        public Task<List<Farm>> GetFarms([ScopedService] AnimalFarmDbContext db) =>
            db.Farms.OrderBy(f => f.Name).ToListAsync();

        public Task<Farm> GetFarmAsync(
            [ID(nameof(Farm))] int id,
            FarmByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public Task<IReadOnlyList<Farm>> GetFarmsAsync(
            [ID(nameof(Farm))] int[] ids,
            FarmByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(ids, cancellationToken);
    }
}