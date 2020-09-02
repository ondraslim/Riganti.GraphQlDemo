using GraphQL.Types;
using RigantiGraphQlDemo.Api.GraphQL.Types;
using RigantiGraphQlDemo.Dal.DataStore;

namespace RigantiGraphQlDemo.Api.GraphQL.Query
{
    public class AppQuery : ObjectGraphType
    {
        public AppQuery(IDataStore dataStore)
        {
            Field<PersonType>()
                .Name("Person")
                .Description("Get Person by Id")
                .Argument<IdGraphType>(Name = "id", Description = "The ID of the Person.")
                .ResolveAsync(async context =>
                {
                    var id = context.GetArgument<int>("id");
                    return await dataStore.GetPersonByIdAsync(id);
                });

            Field<ListGraphType<PersonType>>()
                .Name("Persons")
                .Description("Get all Persons")
                .ResolveAsync(async context => await dataStore.GetPersonsAsync());

            Field<ListGraphType<FarmType>>()
                .Name("Farms")
                .Description("Get all Farms")
                .ResolveAsync(async context => await dataStore.GetFarmsAsync());
        }
    }
}