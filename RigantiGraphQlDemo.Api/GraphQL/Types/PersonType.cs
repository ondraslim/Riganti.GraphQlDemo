using GraphQL.DataLoader;
using GraphQL.Types;
using RigantiGraphQlDemo.Dal.DataStore;
using RigantiGraphQlDemo.Dal.Entities;
using System.Collections.Generic;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class PersonType : ObjectGraphType<Person>
    {
        public PersonType(IDataStore dataStore, IDataLoaderContextAccessor accessor)
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Farm.");
            Field(x => x.Name).Description("The name of the Farm.");
            //Field<ListGraphType<FarmType>, IEnumerable<Farm>>()
            Field<ListGraphType<FarmType>, IEnumerable<Farm>>()
                .Name("Farms")
                .Description("The farms of the Person.")
                //.ResolveAsync(async ctx => await dataStore.GetFarmsByPersonIdAsync(ctx.Source.Id)); // TODO: N+1 problem
            .ResolveAsync(ctx =>
                {
                    var farmLoader = 
                        accessor.Context.GetOrAddCollectionBatchLoader<int, Farm>(
                            "GetFarmsByPersonId", 
                            dataStore.GetFarmsByPersonIdDataLoaderAsync);
            
                    return farmLoader.LoadAsync(ctx.Source.Id);
                }
            );

            // We do not want to expose SecretPiggyBankLocation
            //Field(x => x.SecretPiggyBankLocation).Description("The secret location of person's piggy bank. (should not be available!)");
        }
    }
}