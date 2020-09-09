using GraphQL.DataLoader;
using GraphQL.Types;
using RigantiGraphQlDemo.Api.GraphQL.Types.AnimalTypes;
using RigantiGraphQlDemo.Dal.DataStore.Common;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class FarmType : ObjectGraphType<Farm>
    {
        public FarmType(IDataStore dataStore, IDataLoaderContextAccessor accessor)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Farm.");
            Field(x => x.Name).Description("The name of the Farm.");
            Field(x => x.PersonId).Description("The id of the Farm's owner.");
            Field<PersonType, Person>()
                .Name("Person")
                .Description("Farm's owner.")
                .ResolveAsync(ctx =>
                {
                    var personLoader = accessor.Context.GetOrAddBatchLoader<int, Person>(
                        "GetPersonById",
                        dataStore.GetPersonsByIdDataLoaderAsync);
                    return personLoader.LoadAsync(ctx.Source.PersonId);
                });

            Field(x => x.Animals, type: typeof(ListGraphType<AnimalType>)).Description("Farm's animals.");
        }
    }
}